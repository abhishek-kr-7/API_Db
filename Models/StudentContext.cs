using Microsoft.EntityFrameworkCore;
namespace DbAPI.Models;

public class StudentContext:DbContext
{
    public StudentContext(DbContextOptions<StudentContext> options):base(options){

    }

    public DbSet<Student> Students{get;set;}=null!;
    public List<Student> getStudents() => Students.ToList();

    public DbSet<userCred> userCreds{get;set;}=null!;
    public List<userCred> getuserCreds() => userCreds.ToList();
    
}