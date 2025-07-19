using System.Diagnostics;

namespace TestTask;

static public class DataBaseInfo
{
    private static string alphabet = "QWERTYUIOPASDFGHJKLZXCVBNM";

    private static Random random = new ();

    static public List<EmployeeInfo> GetSortedEmployeesInfo() => DataBase.GetEmployeesDb("SELECT * FROM employees")
        .Distinct().OrderBy(employee => employee.FullName).ToList();

    static public void PrintSortedEmployeesInfo(List<EmployeeInfo> employeeInfos)
    {
        foreach (var employee in employeeInfos)
        {
            Console.WriteLine($"Name: {employee.FullName}, Birthday: {employee.Birthday}, Gender: {employee.EmployeeGender}, Age: {employee.CountAge()}");
        }
    }

    static public List<EmployeeInfo> RandomFillDb1M()
    {
        var listEmployees = new List<EmployeeInfo>();
        for (int i = 0; i < 1000000; ++i)
        {
            var fullname = $"{alphabet[i % alphabet.Length]}{Guid.NewGuid().ToString()}";
            var gender = i % 2 == 0 ? EmployeeInfo.Gender.Male : EmployeeInfo.Gender.Female;
            var birthday = new DateTime(2000, random.Next() % 2 + 10, random.Next() % 20 + 10);
            var employee = new EmployeeInfo(fullname, birthday, gender);
            listEmployees.Add(employee);
        }
        return listEmployees;
    }

    static public List<EmployeeInfo> RandomFillDb100()
    {
        var listEmployees = new List<EmployeeInfo>();
        for (int i = 0; i < 100; ++i)
        {
            var fullname = $"F{Guid.NewGuid().ToString()}";
            var gender = EmployeeInfo.Gender.Male;
            var birthday = new DateTime(2000, random.Next() % 2 + 10, random.Next() % 20 + 10);
            var employee = new EmployeeInfo(fullname, birthday, gender);
            listEmployees.Add(employee);
        }
        return listEmployees;
    }

    static public void SendRandomEmployeeDb(List<EmployeeInfo> listEmployees)
    {
        for (int i = 0; i < listEmployees.Count; i += 1000)
        {
            int count = Math.Min(1000, listEmployees.Count - i);
            DataBase.AddRecordDb(listEmployees.GetRange(i, count));
        }
    }

    ///Result of selection from the table by criterion after optimization (lead time approx 0.3 seconds for 1000100 records)
    static public ValueTuple<long, List<EmployeeInfo>> GetMaleFiltered()
    {
        var sw = new Stopwatch();
        sw.Start();
        var list = DataBase.GetEmployeesDb("SELECT * FROM employees WHERE gender = 'Male' AND left(full_name, 1) = 'F'");
        sw.Stop();
        var elapsedTime = sw.ElapsedMilliseconds;
        return new ValueTuple<long, List<EmployeeInfo>>(elapsedTime, list);
    }

    ///Result of selection from the table by criterion before optimization (lead time approx 1.6 seconds for 1000100 records)
    /*static public ValueTuple<long, List<EmployeeInfo>> GetMaleFiltered()
    {
    var sw = new Stopwatch();
    sw.Start();
    var list = DataBase.GetEmployeesDb("SELECT * FROM employees")
     .Where(e => e.EmployeeGender == EmployeeInfo.Gender.Male && e.FullName.StartsWith("F")).ToList();
    sw.Stop();
    var elapsedTime = sw.ElapsedMilliseconds;
    return new ValueTuple<long, List<EmployeeInfo>>(elapsedTime, list);
    }*/

}
