using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.Services;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using Moq;


namespace UnitTests.ServiceTests;

[TestClass]
public class ReportServiceTest
{
    [TestMethod]
    public async Task Create_Should_Return_Success_Response()
    {
        //Arrange
        var mockRepo = new Mock<IUnitOfWork>();
        mockRepo.Setup(repo => repo.ReportRepository.Create(It.IsAny<Report>())).Verifiable();
        mockRepo.Setup(repo => repo.CommitAsync()).ReturnsAsync(1);

        var service = new ReportService(mockRepo.Object);

        var dto = GetTestReportDTO();

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_SAVE
        };

        //Act
        var response = await service.Create(dto);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        mockRepo.Verify(uow => uow.ReportRepository.Create(It.IsAny<Report>()), Times.Once);
        mockRepo.Verify(uow => uow.CommitAsync(), Times.Once);

    }

    [TestMethod]
    public async Task GetAllReportsByDoctor_Should_Return_Success_Response_With_Reports()
    {
        // Arrange
        var doctorId = 1;

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.GetAllReportsByDoctor(It.IsAny<int>()))
            .ReturnsAsync(GetReports());

        var reportService = new ReportService(unitOfWorkMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<ReportDTO>>
        {
            Success = true,
            Data = GetTestReports(),
            Message = ReplyMessage.MESSAGE_QUERY
        };

        // Act
        var response = await reportService.GetAllReportsByDoctor(doctorId); 

        // Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        // Compare Data
        var expectedData = expectedResponse.Data.ToList();
        var responseData = response.Data!.ToList();

        Assert.AreEqual(expectedData.Count, responseData.Count);
        
        for(var i=0; i<expectedData.Count; i++)
        {
            Assert.AreEqual(expectedData[i].Id, responseData[i].Id);  
            Assert.AreEqual(expectedData[i].Diagnosis, responseData[i].Diagnosis);  
        }

    }

    [TestMethod]
    public async Task GetAllReportsByDoctor_Should_Return_Failure_Response_When_Reports_Are_Not_Found()
    {
        // Arrange
        var doctorId = 1;

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.GetAllReportsByDoctor(doctorId)).ReturnsAsync((List<Report>)null);

        var reportService = new ReportService(unitOfWorkMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<ReportDTO>>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        // Act
        var response = await reportService.GetAllReportsByDoctor(doctorId);

        // Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);
    }

    [TestMethod]
    public async Task GetAllReportsByHospital_Should_Return_Success_Response_With_Reports()
    {
        //Arrange
        var hospitalId = 1;

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.GetAllReportsByHospital(It.IsAny<int>()))
            .ReturnsAsync(GetReports());

        var reportService = new ReportService(unitOfWorkMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<ReportDTO>>
        {
            Success = true,
            Data = GetTestReports(),
            Message = ReplyMessage.MESSAGE_QUERY
        };

        //Act
        var response = await reportService.GetAllReportsByHospital(hospitalId);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        // Compare Data
        var expectedData = expectedResponse.Data.ToList();
        var responseData = response.Data!.ToList();

        Assert.AreEqual(expectedData.Count, responseData.Count);

        for (var i = 0; i < expectedData.Count; i++)
        {
            Assert.AreEqual(expectedData[i].Id, responseData[i].Id);
            Assert.AreEqual(expectedData[i].Diagnosis, responseData[i].Diagnosis);
        }

    }

    [TestMethod]
    public async Task GetAllReportsByHospital_Should_Return_Failure_Response_When_Reports_Are_Not_Found()
    {
        // Arrange
        var doctorId = 1;

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.GetAllReportsByHospital(It.IsAny<int>()))
            .ReturnsAsync((List<Report>?)null);

        var reportService = new ReportService(unitOfWorkMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<ReportDTO>>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        // Act
        var response = await reportService.GetAllReportsByHospital(doctorId);

        // Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);
    }

    [TestMethod]
    public async Task GetAllReportsByPatient_Should_Return_Success_Response_With_Reports()
    {
        //Arrange
        var hospitalId = 1;

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.GetAllReportsByPatient(It.IsAny<int>()))
            .ReturnsAsync(GetReports());

        var reportService = new ReportService(unitOfWorkMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<ReportDTO>>
        {
            Success = true,
            Data = GetTestReports(),
            Message = ReplyMessage.MESSAGE_QUERY
        };

        //Act
        var response = await reportService.GetAllReportsByPatient(hospitalId);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        // Compare Data
        var expectedData = expectedResponse.Data.ToList();
        var responseData = response.Data!.ToList();

        Assert.AreEqual(expectedData.Count, responseData.Count);

        for (var i = 0; i < expectedData.Count; i++)
        {
            Assert.AreEqual(expectedData[i].Id, responseData[i].Id);
            Assert.AreEqual(expectedData[i].Diagnosis, responseData[i].Diagnosis);
        }

    }

    [TestMethod]
    public async Task GetAllReportsByPatient_Should_Return_Failure_Response_When_Reports_Are_Not_Found()
    {
        // Arrange
        var doctorId = 1;

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.GetAllReportsByPatient(It.IsAny<int>()))
            .ReturnsAsync((List<Report>?)null);

        var reportService = new ReportService(unitOfWorkMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<ReportDTO>>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        // Act
        var response = await reportService.GetAllReportsByPatient(doctorId);

        // Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);
    }

    [TestMethod]
    public async Task Update_Should_Return_Success_Response()
    {
        //Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.Update(It.IsAny<Report>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var reportService = new ReportService(unitOfWorkMock.Object);

        var dto = GetTestReportDTO();

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_UPDATE
        };

        //Act
        var response = await reportService.Update(dto);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.ReportRepository.Update(It.IsAny<Report>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);


    }

    [TestMethod]
    public async Task Remove_Should_Return_Success_Response()
    {

        //Arrange
        var reportId = 1;

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.ReportRepository.Remove(It.IsAny<int>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var reportService = new ReportService(unitOfWorkMock.Object);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_DELETE
        };

        //Act
        var response = await reportService.Remove(reportId);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.ReportRepository.Remove(It.IsAny<int>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }


    private IEnumerable<ReportDTO> GetTestReports()
    {
        // Return a list of test reports for testing purposes
        return new List<ReportDTO>
        {
            new ReportDTO { Id = 1, Diagnosis = "Report 1" },
            new ReportDTO { Id = 2, Diagnosis = "Report 2" },
            new ReportDTO { Id = 3, Diagnosis = "Report 3" }
        };
    }

    private IEnumerable<Report> GetReports()
    {
        // Return a list of test reports for testing purposes
        return new List<Report>
        {
            new Report { Id = 1, Diagnosis = "Report 1" },
            new Report { Id = 2, Diagnosis = "Report 2" },
            new Report { Id = 3, Diagnosis = "Report 3" }
        };
    }

    private ReportDTO GetTestReportDTO()
    {
        return new ReportDTO
        {
            Diagnosis = "Enfermo de cancer",
            Observation = "Debe entrar en tratamiento",
            Treatment = "Quimioterapia",
            Doctor = new DoctorNameDTO
            {
                Id = 1,
                Name = "Doctor1",
                MedicalSpecialty = "Oncología"
            },
            Patient = new PatientDTO
            {
                Id = 1,
                Name = "Patient1",
                Email = "patient1@email.com",
                Identification = "1146339245",
                PhoneNumber = "30541355",
                Address = "Cll 1, Mz 20",
                BirthDate = new DateTime(2011, 06, 24),

            },
            Hospital = new HospitalNameDTO
            {
                Id = 1,
                Name = "Hospital1"
            }
        };
    }
}
