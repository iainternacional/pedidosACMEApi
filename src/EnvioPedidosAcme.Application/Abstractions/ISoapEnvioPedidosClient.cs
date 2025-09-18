using EnvioPedidosAcme.Application.Contracts;
namespace EnvioPedidosAcme.Application.Abstractions;
public interface ISoapEnvioPedidosClient
{
    Task<EnviarPedidoJsonResponse> EnviarPedidoAsync(EnviarPedidoJsonRequest request, CancellationToken ct = default);
}
