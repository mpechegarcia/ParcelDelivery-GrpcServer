# ParcelDelivery-GrpcServer
# ParcelDelivery-GrpcServer

C# gRPC server for our SEP3 Parcel Delivery Tracking Proof of Concept.

## Technologies

* C#
* .NET 10
* gRPC
* Protocol Buffers

## Features

The server currently supports three operations:

* **CreateShipment:** Creates a new shipment.
* **GetShipment:** Finds a shipment using its tracking number.
* **UpdateShipmentStatus:** Updates the status of a shipment.

It also handles basic errors, such as when a shipment is not found.

## How to run

Open the project in Rider and run:

```bash
dotnet run --project ParcelDelivery.GrpcServer
```

The server runs locally on `http://localhost:5187`.

You can test the gRPC methods using Postman and the `shipment.proto` file.

## Current status

The three gRPC methods are implemented and tested.

For now, shipments are stored in memory. PostgreSQL persistence and integration with the Java Spring Boot server are still pending.

