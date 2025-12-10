using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.Models;
using StudentService.Services;
using System.Text.Json;

namespace StudentService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentDbContext _context;
    private readonly IServiceBusService _serviceBusService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(
        StudentDbContext context,
        IServiceBusService serviceBusService,
        ILogger<StudentController> logger)
    {
        _context = context;
        _serviceBusService = serviceBusService;
        _logger = logger;
    }

    /// <summary>
    /// Get all students
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDetailsDto>>> GetAllStudents()
    {
        try
        {
            _logger.LogInformation("Fetching all students");

            var students = await _context.Students
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            var studentDtos = students.Select(MapToDto).ToList();

            _logger.LogInformation("Successfully fetched {Count} students", studentDtos.Count);
            return Ok(studentDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all students");
            return StatusCode(500, new { message = "An error occurred while fetching students" });
        }
    }

    /// <summary>
    /// Get student by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDetailsDto>> GetStudent(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching student with ID: {StudentId}", id);

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Student not found with ID: {StudentId}", id);
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            var studentDto = MapToDto(student);

            _logger.LogInformation("Successfully fetched student with ID: {StudentId}", id);
            return Ok(studentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching student with ID: {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the student" });
        }
    }

    /// <summary>
    /// Save new student
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<StudentDetailsDto>> SaveStudent([FromBody] StudentDetailsDto studentDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for student creation");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new student: {FirstName} {LastName}",
                studentDto.FirstName, studentDto.LastName);

            var student = MapToEntity(studentDto);
            student.Id = Guid.NewGuid();
            student.CreatedAt = DateTime.UtcNow;

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Student created successfully with ID: {StudentId}", student.Id);

            // Send message to Service Bus
            try
            {
                var message = JsonSerializer.Serialize(new
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DOB = student.DOB,
                    Gender = student.Gender,
                    CreatedAt = student.CreatedAt,
                    EventType = "StudentCreated"
                });

                await _serviceBusService.SendMessageAsync(message);
                _logger.LogInformation("Student creation message sent to Service Bus for ID: {StudentId}", student.Id);
            }
            catch (Exception sbEx)
            {
                _logger.LogError(sbEx, "Failed to send message to Service Bus for student ID: {StudentId}", student.Id);
                // Continue execution - we don't want to fail the request if Service Bus fails
            }

            var resultDto = MapToDto(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");
            return StatusCode(500, new { message = "An error occurred while saving the student" });
        }
    }

    /// <summary>
    /// Update existing student
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDetailsDto>> UpdateStudent(Guid id, [FromBody] StudentDetailsDto studentDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for student update");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating student with ID: {StudentId}", id);

            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                _logger.LogWarning("Student not found with ID: {StudentId}", id);
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            // Update properties
            existingStudent.FirstName = studentDto.FirstName;
            existingStudent.LastName = studentDto.LastName;
            existingStudent.DOB = DateTime.Parse(studentDto.DOB);
            existingStudent.Gender = studentDto.Gender;

            // Update Address 1
            existingStudent.Address1_Address1 = studentDto.Address1.Address1;
            existingStudent.Address1_Address2 = studentDto.Address1.Address2;
            existingStudent.Address1_City = studentDto.Address1.City;
            existingStudent.Address1_State = studentDto.Address1.State;
            existingStudent.Address1_Zipcode = studentDto.Address1.Zipcode;

            // Update Address 2
            existingStudent.Address2_Address1 = studentDto.Address2.Address1;
            existingStudent.Address2_Address2 = studentDto.Address2.Address2;
            existingStudent.Address2_City = studentDto.Address2.City;
            existingStudent.Address2_State = studentDto.Address2.State;
            existingStudent.Address2_Zipcode = studentDto.Address2.Zipcode;

            // Update Address 3
            existingStudent.Address3_Address1 = studentDto.Address3.Address1;
            existingStudent.Address3_Address2 = studentDto.Address3.Address2;
            existingStudent.Address3_City = studentDto.Address3.City;
            existingStudent.Address3_State = studentDto.Address3.State;
            existingStudent.Address3_Zipcode = studentDto.Address3.Zipcode;

            // Update Address 4
            existingStudent.Address4_Address1 = studentDto.Address4.Address1;
            existingStudent.Address4_Address2 = studentDto.Address4.Address2;
            existingStudent.Address4_City = studentDto.Address4.City;
            existingStudent.Address4_State = studentDto.Address4.State;
            existingStudent.Address4_Zipcode = studentDto.Address4.Zipcode;

            existingStudent.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Student updated successfully with ID: {StudentId}", id);

            var resultDto = MapToDto(existingStudent);
            return Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student with ID: {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the student" });
        }
    }

    /// <summary>
    /// Delete student
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting student with ID: {StudentId}", id);

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student not found with ID: {StudentId}", id);
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Student deleted successfully with ID: {StudentId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student with ID: {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the student" });
        }
    }

    private static StudentDetailsDto MapToDto(StudentDetails student)
    {
        return new StudentDetailsDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            DOB = student.DOB.ToString("yyyy-MM-dd"),
            Gender = student.Gender,
            Address1 = new Address
            {
                Address1 = student.Address1_Address1,
                Address2 = student.Address1_Address2,
                City = student.Address1_City,
                State = student.Address1_State,
                Zipcode = student.Address1_Zipcode
            },
            Address2 = new Address
            {
                Address1 = student.Address2_Address1,
                Address2 = student.Address2_Address2,
                City = student.Address2_City,
                State = student.Address2_State,
                Zipcode = student.Address2_Zipcode
            },
            Address3 = new Address
            {
                Address1 = student.Address3_Address1,
                Address2 = student.Address3_Address2,
                City = student.Address3_City,
                State = student.Address3_State,
                Zipcode = student.Address3_Zipcode
            },
            Address4 = new Address
            {
                Address1 = student.Address4_Address1,
                Address2 = student.Address4_Address2,
                City = student.Address4_City,
                State = student.Address4_State,
                Zipcode = student.Address4_Zipcode
            }
        };
    }

    private static StudentDetails MapToEntity(StudentDetailsDto dto)
    {
        return new StudentDetails
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DOB = DateTime.Parse(dto.DOB),
            Gender = dto.Gender,
            Address1_Address1 = dto.Address1.Address1,
            Address1_Address2 = dto.Address1.Address2,
            Address1_City = dto.Address1.City,
            Address1_State = dto.Address1.State,
            Address1_Zipcode = dto.Address1.Zipcode,
            Address2_Address1 = dto.Address2.Address1,
            Address2_Address2 = dto.Address2.Address2,
            Address2_City = dto.Address2.City,
            Address2_State = dto.Address2.State,
            Address2_Zipcode = dto.Address2.Zipcode,
            Address3_Address1 = dto.Address3.Address1,
            Address3_Address2 = dto.Address3.Address2,
            Address3_City = dto.Address3.City,
            Address3_State = dto.Address3.State,
            Address3_Zipcode = dto.Address3.Zipcode,
            Address4_Address1 = dto.Address4.Address1,
            Address4_Address2 = dto.Address4.Address2,
            Address4_City = dto.Address4.City,
            Address4_State = dto.Address4.State,
            Address4_Zipcode = dto.Address4.Zipcode
        };
    }
}
