namespace TestTask;

class Program
{
    public static void Main(string[] args)
    {
        switch (args[0])
        {
            case "1":
                try
                {
                    DataBase.CreateTable();
                    Console.WriteLine("Operation completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "2":
                try
                {
                    var name = args[1];
                    var date = args[2].Split("-");
                    var gender = args[3] == "Male" ? EmployeeInfo.Gender.Male : EmployeeInfo.Gender.Female;
                    var employee = new EmployeeInfo(name, new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2])), gender);
                    employee.SendToDb();
                    Console.WriteLine("Operation completed");
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Error app parameters");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "3":
                try
                {
                    DataBaseInfo.PrintSortedEmployeesInfo(DataBaseInfo.GetSortedEmployeesInfo());
                    Console.WriteLine("Operation completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "4":
                try
                {
                    DataBaseInfo.SendRandomEmployeeDb(DataBaseInfo.RandomFillDb1M());
                    DataBaseInfo.SendRandomEmployeeDb(DataBaseInfo.RandomFillDb100());
                    Console.WriteLine("Operation completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "5":
                try
                {
                    var a = DataBaseInfo.GetMaleFiltered();
                    DataBaseInfo.PrintSortedEmployeesInfo(DataBaseInfo.GetMaleFiltered().Item2);
                    Console.WriteLine($"Time execution - {a.Item1}");
                    Console.WriteLine("Operation completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            default:
                Console.WriteLine("Error app parameters");
                break;
        }
    }
}
