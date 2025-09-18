namespace EnvioPedidosAcme.Application.Contracts;
public sealed class EnviarPedidoJsonRequest
{
    public required EnviarPedidoPayload enviarPedido { get; set; } = default!;
}
public sealed class EnviarPedidoPayload
{
    public required string numPedido { get; set; } = default!;
    public required string cantidadPedido { get; set; } = default!;
    public required string codigoEAN { get; set; } = default!;
    public required string nombreProducto { get; set; } = default!;
    public required string numDocumento { get; set; } = default!;
    public required string direccion { get; set; } = default!;
}
