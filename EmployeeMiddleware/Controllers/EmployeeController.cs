using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMiddleware.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl = "http://localhost:5000/api/employee";

    public EmployeeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get all employees
    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var response = await _httpClient.GetAsync(_apiBaseUrl);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var employees = JsonSerializer.Deserialize<IEnumerable<Employee.Models.Employee>>(responseString, options);

            if (employees == null)
            {
                return StatusCode(500, "Failed to deserialize employees");
            }

            return Ok(employees);
        }
        return StatusCode((int)response.StatusCode, "Error fetching data from API");
    }

    // Get employee by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var employee = JsonSerializer.Deserialize<Employee.Models.Employee>(responseString, options);

            if (employee == null)
            {
                return StatusCode(500, "Failed to deserialize employee");
            }

            return Ok(employee);
        }
        return NotFound();
    }

    // Add new employee
    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] Employee.Models.Employee employee)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, employee);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var newEmployee = JsonSerializer.Deserialize<Employee.Models.Employee>(responseString, options);

            if (newEmployee == null)
            {
                return StatusCode(500, "Failed to deserialize new employee");
            }

            return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.Id }, newEmployee);
        }
        return BadRequest("Error adding employee");
    }

    // Update employee
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee.Models.Employee employee)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/{id}", employee);

        if (response.IsSuccessStatusCode)
        {
            return NoContent();
        }
        return NotFound();
    }

    // Delete employee
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            return NoContent();
        }
        return NotFound();
    }
}
