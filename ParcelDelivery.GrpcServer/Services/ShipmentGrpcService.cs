using Grpc.Core;
using ParcelDelivery.GrpcServer;

namespace ParcelDelivery.GrpcServer.Services;

public class ShipmentGrpcService : ShipmentService.ShipmentServiceBase {
    private static readonly Dictionary<string, ShipmentResponse> Shipments = new();

    private static readonly object Lock = new();

    public override Task<ShipmentResponse> CreateShipment(
        CreateShipmentRequest request,
        ServerCallContext context) {
        if (string.IsNullOrWhiteSpace(request.Sender) ||
            string.IsNullOrWhiteSpace(request.Recipient)) {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                "Sender and recipient are required"));
        }

        var shipment = new ShipmentResponse {
            TrackingNumber = Guid.NewGuid().ToString("N"),
            Sender = request.Sender,
            Recipient = request.Recipient,
            Status = "Registered",
            CreatedAt = DateTime.UtcNow.ToString("O")
        };

        lock (Lock) {
            Shipments.Add(shipment.TrackingNumber, shipment);
        }

        return Task.FromResult(shipment);
    }

    public override Task<ShipmentResponse> GetShipment(
        GetShipmentRequest request,
        ServerCallContext context) {
        lock (Lock) {
            if (!Shipments.TryGetValue(
                    request.TrackingNumber, out var shipment)) {
                throw new RpcException(new Status(
                    StatusCode.NotFound,
                    "Shipment not found"));
            }

            return Task.FromResult(shipment.Clone());
        }
    }

    public override Task<ShipmentResponse> UpdateShipmentStatus(
        UpdateShipmentStatusRequest request,
        ServerCallContext context) {
        if (string.IsNullOrWhiteSpace(request.Status)) {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                "Status is required"));
        }

        lock (Lock) {
            if (!Shipments.TryGetValue(
                    request.TrackingNumber, out var shipment)) {
                throw new RpcException(new Status(
                    StatusCode.NotFound,
                    "Shipment not found"));
            }

            shipment.Status = request.Status;

            return Task.FromResult(shipment.Clone());
        }
    }
}