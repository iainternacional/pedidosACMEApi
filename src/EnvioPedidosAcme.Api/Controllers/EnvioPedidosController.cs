using EnvioPedidosAcme.Application.Abstractions;
using EnvioPedidosAcme.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
namespace EnvioPedidosAcme.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public sealed class EnvioPedidosController : ControllerBase
{
    private readonly ISoapEnvioPedidosClient _client;
    public EnvioPedidosController(ISoapEnvioPedidosClient client) => _client = client;
    [HttpPost]
    public async Task<ActionResult<EnviarPedidoJsonResponse>> Post([FromBody] EnviarPedidoJsonRequest req, CancellationToken ct)
    {
        if (req?.enviarPedido == null) return BadRequest();
        return Ok(await _client.EnviarPedidoAsync(req, ct));
    }
}
