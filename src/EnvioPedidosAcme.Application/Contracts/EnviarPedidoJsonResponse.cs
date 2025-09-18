namespace EnvioPedidosAcme.Application.Contracts;
public sealed class EnviarPedidoJsonResponse
{
    public required EnviarPedidoRespuesta enviarPedidoRespuesta { get; set; } = default!;
}
public sealed class EnviarPedidoRespuesta
{
    public required string codigoEnvio { get; set; } = default!;
    public required string estado { get; set; } = default!;
}
