namespace TestTask;

public class EmployeeInfo
{
    public enum Gender
    {
        Male,
        Female
    }

    public string FullName { get; private set; }

    public DateTime Birthday { get; private set; }

    public Gender EmployeeGender { get; private set; }

    public EmployeeInfo(string fullname, DateTime birthday, Gender gender)
    {
        this.FullName = fullname;
        this.Birthday = birthday;
        this.EmployeeGender = gender;
    }

    public void SendToDb() => DataBase.AddRecordDb(new EmployeeInfo[] { this });

    public int CountAge()
    {
        if (DateTime.Now.Month > Birthday.Month || DateTime.Now.Month == Birthday.Month && DateTime.Now.Day >= Birthday.Day)
        {
            return DateTime.Now.Year - Birthday.Year;
        }
        return DateTime.Now.Year - Birthday.Year - 1;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        EmployeeInfo secondObject = (EmployeeInfo)obj;
        return FullName == secondObject.FullName && Birthday == secondObject.Birthday;
    }

    public override int GetHashCode() => HashCode.Combine(FullName, Birthday);
}
