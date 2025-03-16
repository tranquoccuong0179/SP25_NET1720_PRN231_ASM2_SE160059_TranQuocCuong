using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Psychologicaly.grpc.Protos;
namespace Psychologicaly.grpc.Client;
internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Welcome to gRPC Client!");

        using var channel = GrpcChannel.ForAddress("https://localhost:7058");
        var client = new AppointmentService.AppointmentServiceClient(channel);

        Console.WriteLine("Get All Appointment");
        var appointments = client.GetAll(new Protos.Empty());
        if (appointments != null && appointments.AppointmentReport.Count > 0)
        {
            foreach (var appointment in appointments.AppointmentReport)
            {
                Console.WriteLine($"{appointment.Id} - {appointment.Description}");
            }
        }
        else
        {
            Console.WriteLine("No Appointment");
        }

        Console.WriteLine("\r\nRPCClient.GetById(Id=1):");
        var appointmentById = client.GetById(new AppointmentReportIdRequest { Id = 1 });
        Console.WriteLine($"{appointmentById.Id} - {appointmentById.Description}");

        // ⬇ Thêm dòng này để giữ console mở
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }

}