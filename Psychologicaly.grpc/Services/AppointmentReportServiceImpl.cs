
using Grpc.Core;
using System.Text.Json.Serialization;
using System.Text.Json;
using Psychologicaly.grpc.Protos;
using Psychologicaly.Service;

namespace Psychologicaly.grpc.Services
{
    public class AppointmentReportServiceImpl : Protos.AppointmentService.AppointmentServiceBase
    {
        public readonly IAppointmentReportService appointmentReportService;
        private readonly ILogger<AppointmentReportServiceImpl> _logger;
        private static List<Protos.AppointmentReport> _appointmentReportService = new List<Protos.AppointmentReport>
     {
         new Protos.AppointmentReport {
         Id = 1,
         AppointmentId = 1,
         Description = "Kiem tra 1",
         Star = 1,
         CreateAt = DateTime.Now.ToString(),
         UpdateAt = DateTime.Now.ToString()
         },
         new Protos.AppointmentReport() {
         Id = 2,
         AppointmentId = 1,
         Description = "Kiem tra 2",
         Star = 2,
         CreateAt = DateTime.Now.ToString(),
         UpdateAt = DateTime.Now.ToString()
         }
     };


        public AppointmentReportServiceImpl(ILogger<AppointmentReportServiceImpl> logger)
        {
            _logger = logger;
        }

        public override async Task<AppointmentReportList> GetAll(Protos.Empty request, ServerCallContext context)
        {
            var response = new AppointmentReportList();
            response.AppointmentReport.AddRange(_appointmentReportService);

            return await Task.FromResult(response);
        }

        public override Task<Protos.AppointmentReport> GetById(AppointmentReportIdRequest request, ServerCallContext context)
        {
            var appointment = _appointmentReportService.FirstOrDefault(s => s.Id == request.Id);
            if (appointment == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Appointment not found"));
            }
            return Task.FromResult(appointment);
        }

        public override Task<ActionResult> DeleteById(AppointmentReportIdRequest request, ServerCallContext context)
        {
            var item = _appointmentReportService.FirstOrDefault(b => b.Id == request.Id);
            if (item != null)
            {
                _appointmentReportService.Remove(item);
                return Task.FromResult(new ActionResult() { Status = 1, Message = "Delete success", Data = new AppointmentReportList() { AppointmentReport = { _appointmentReportService } } });
            }

            return Task.FromResult(new ActionResult() { Status = -1, Message = "Delete fail" });
        }

        public override Task<ActionResult> Add(Protos.AppointmentReport request, ServerCallContext context)
        {
            if (request != null)
            {
                _appointmentReportService.Add(request);
                return Task.FromResult(new ActionResult() { Status = 1, Message = "Add success", Data = new AppointmentReportList() { AppointmentReport = { _appointmentReportService } } });
            }

            return Task.FromResult(new ActionResult() { Status = -1, Message = "Add fail" });
        }

        public override Task<ActionResult> Edit(Protos.AppointmentReport request, ServerCallContext context)
        {
            if (request != null)
            {
                var item = _appointmentReportService.FirstOrDefault(b => b.Id == request.Id);
                if (item != null)
                {
                    _appointmentReportService.Remove(item);
                    _appointmentReportService.Remove(request);

                    return Task.FromResult(new ActionResult() { Status = 1, Message = "Edit success", Data = new AppointmentReportList() { AppointmentReport = { _appointmentReportService } } });
                }
            }

            return Task.FromResult(new ActionResult() { Status = -1, Message = "Edit fail" });
        }

        public override async Task<ActionResult> AddAsync(Protos.AppointmentReport request, ServerCallContext context)
        {
            try
            {
                if (request != null)
                {
                    var opt = new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };

                    var appointmentString = JsonSerializer.Serialize(request, opt);
                    var item = JsonSerializer.Deserialize<Repository.Models.AppointmentReport>(appointmentString, opt);

                    var result = await appointmentReportService.Create(item);

                    if (result > 0)
                    {
                        return await Task.FromResult(new ActionResult() { Status = 1, Message = "Add Success" });
                    }
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ActionResult() { Status = -4, Message = string.Format("Add Fail. (0)", ex.ToString()) });

            }
            return await Task.FromResult(new ActionResult() { Status = -4, Message = "Add Fail" });
        }

        public override async Task<AppointmentReportList> GetAllAsync(Protos.Empty request, ServerCallContext context)
        {
            try
            {
                var result = new AppointmentReportList();
                var appointmentReports = await appointmentReportService.GetAll();

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler =
                    ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull
                };

                var appointmentString = JsonSerializer.Serialize(appointmentReports, opt);
                var items = JsonSerializer.Deserialize<List<Protos.AppointmentReport>>(appointmentString, opt);

                result.AppointmentReport.AddRange(items);

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return new AppointmentReportList();

            }
        }

        public override async Task<Protos.AppointmentReport> GetByIdAsync(AppointmentReportIdRequest request, ServerCallContext context)
        {
            try
            {
                var appointment = await appointmentReportService.GetById(request.Id);

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler =
                    ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull
                };

                var appointmentString = JsonSerializer.Serialize(appointment, opt);
                var items = JsonSerializer.Deserialize<Protos.AppointmentReport>(appointmentString, opt);

                return await Task.FromResult(items);
            }
            catch (Exception ex)
            {
                return new Protos.AppointmentReport();
            }
        }

        public override async Task<ActionResult> EditAsync(Protos.AppointmentReport request, ServerCallContext context)
        {
            try
            {
                if (request != null)
                {
                    var opt = new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };

                    var appointmentString = JsonSerializer.Serialize(request, opt);
                    var item = JsonSerializer.Deserialize<Repository.Models.AppointmentReport>(appointmentString, opt);

                    var result = await appointmentReportService.Update(item);

                    if (result > 0)
                    {
                        return await Task.FromResult(new ActionResult() { Status = 1, Message = "Update Success" });
                    }
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ActionResult() { Status = -4, Message = string.Format("Update Fail. (0)", ex.ToString()) });

            }
            return await Task.FromResult(new ActionResult() { Status = -4, Message = "Update Fail" });
        }

        public override async Task<ActionResult> DeleteByIdAsync(AppointmentReportIdRequest request, ServerCallContext context)
        {
            try
            {
                var appointment = await appointmentReportService.Remove(request.Id);

                return await Task.FromResult(new ActionResult() { Status = 1, Message = "Delete success" });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ActionResult() { Status = -1, Message = string.Format("Delete Fail .(0)", ex.ToString()) });
            }
        }
    }
}
