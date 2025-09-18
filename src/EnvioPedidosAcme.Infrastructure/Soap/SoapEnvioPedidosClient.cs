using System.Xml.Linq;
using EnvioPedidosAcme.Application.Abstractions;
using EnvioPedidosAcme.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EnvioPedidosAcme.Infrastructure.Soap;

public sealed class SoapEnvioPedidosClient : ISoapEnvioPedidosClient
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<SoapEnvioPedidosClient> _logger;

    public SoapEnvioPedidosClient(IHttpClientFactory httpFactory, IConfiguration config, ILogger<SoapEnvioPedidosClient> logger)
    {
        _httpFactory = httpFactory;
        _config = config;
        _logger = logger;
    }

    public async Task<EnviarPedidoJsonResponse> EnviarPedidoAsync(EnviarPedidoJsonRequest request, CancellationToken ct = default)
    {
        var endpoint = _config["SoapEnvioPedidos:Endpoint"]
                       ?? throw new InvalidOperationException("Soap endpoint not configured");
        var tns = _config["SoapEnvioPedidos:Namespace"]
                  ?? "http://WSDLs/EnvioPedidos/EnvioPedidosAcme";

        XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
        XNamespace ns = tns;

        // Construir envelope document/literal según WSDL
        var xdoc = new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv),
                new XAttribute(XNamespace.Xmlns + "tns", ns),
                new XElement(soapenv + "Header"),
                new XElement(soapenv + "Body",
                    new XElement(ns + "EnvioPedidoAcme",
                        new XElement(ns + "EnvioPedidoRequest",
                            new XElement(ns + "pedido", request.enviarPedido.numPedido),
                            new XElement(ns + "Cantidad", request.enviarPedido.cantidadPedido),
                            new XElement(ns + "EAN", request.enviarPedido.codigoEAN),
                            new XElement(ns + "Producto", request.enviarPedido.nombreProducto),
                            new XElement(ns + "Cedula", request.enviarPedido.numDocumento),
                            new XElement(ns + "Direccion", request.enviarPedido.direccion)
                        )
                    )
                )
            )
        );

        var xmlString = xdoc.ToString(SaveOptions.DisableFormatting);
        _logger.LogInformation("SOAP Request: {xml}", xmlString);

        using var client = _httpFactory.CreateClient();
        using var req = new HttpRequestMessage(HttpMethod.Post, endpoint);
        req.Content = new StringContent(xmlString, System.Text.Encoding.UTF8, "text/xml");
        // *** SOAPAction obligatorio según tu WSDL ***
        req.Headers.Add("SOAPAction", $"{tns}/EnvioPedidoAcme");

        using var resp = await client.SendAsync(req, ct);
        var respXml = await resp.Content.ReadAsStringAsync(ct);
        _logger.LogInformation("SOAP Response: {xml}", respXml);
        resp.EnsureSuccessStatusCode();

        // Parseo de respuesta
        var xd = XDocument.Parse(respXml);
        var body = xd.Root?.Element(soapenv + "Body");
        var opResp = body?.Element(ns + "EnvioPedidoAcmeResponse");
        var payload = opResp?.Element(ns + "EnvioPedidoResponse");

        var codigo = payload?.Element(ns + "Codigo")?.Value ?? string.Empty;
        var mensaje = payload?.Element(ns + "Mensaje")?.Value ?? string.Empty;

        return new EnviarPedidoJsonResponse
        {
            enviarPedidoRespuesta = new EnviarPedidoRespuesta
            {
                codigoEnvio = codigo,
                estado = mensaje
            }
        };
    }
}
