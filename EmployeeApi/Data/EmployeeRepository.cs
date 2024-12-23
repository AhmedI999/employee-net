using System.Data;
using Dapper;
using Npgsql;

namespace Employee.Data
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        
        public async Task<IEnumerable<Models.Employee>> GetAllEmployeesAsync()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = "SELECT * FROM employee";
                    return await connection.QueryAsync<Models.Employee>(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all employees: " + ex.Message, ex);
            }
        }
        
        public async Task<Models.Employee> GetEmployeeByIdAsync(int id)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = "SELECT * FROM employee WHERE id = @id";
                    return await connection.QueryFirstOrDefaultAsync<Models.Employee>(query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching employee with Id {id}: {ex.Message}", ex);
            }
        }
        public async Task<Models.Employee> AddEmployeeAsync(Models.Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Name) || string.IsNullOrEmpty(employee.Position))
            {
                throw new ArgumentException("Name and Position cannot be empty");
            }

            try
            {
                using (var connection = CreateConnection())
                {
                    string query = @"INSERT INTO employee (name, position, salary)
                                     VALUES (@name, @position, @salary)
                                     RETURNING id";
                    var id = await connection.ExecuteScalarAsync<int>(query, employee);
                    employee.Id = id;
                    return employee;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new employee: " + ex.Message, ex);
            }
        }
        
        public async Task UpdateEmployeeAsync(int id, Models.Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Name) || string.IsNullOrEmpty(employee.Position))
            {
                throw new ArgumentException("Name and Position cannot be empty");
            }

            try
            {
                using (var connection = CreateConnection())
                {
                    string query = @"
                        UPDATE employee
                        SET name = @name, position = @position, salary = @salary
                        WHERE id = @id";
                    
                    await connection.ExecuteAsync(query, new
                    {
                        employee.Name,
                        employee.Position,
                        employee.Salary,
                        Id = id
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee with Id {id}: {ex.Message}", ex);
            }
        }
        
        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    string query = "DELETE FROM employee WHERE id = @id";
                    await connection.ExecuteAsync(query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting employee with Id {id}: {ex.Message}", ex);
            }
        }
    }
}
