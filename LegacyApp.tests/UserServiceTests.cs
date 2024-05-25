namespace LegacyApp.tests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_ReturnsFalseWhenFirstNameIsEmpty()
    {
        // Arrange
        var UserService = new UserService();

        // Act
        var result = UserService.AddUser(null, "Kowalski", "kowal@gmail.com", DateTime.Parse("2000-05-05"), 51);

        // Asert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_AgeBelow21YearsOld()
    {
        // Arrange
        var UserService = new UserService();

        // Act
        var result = UserService.AddUser("Jan", "Kowalski", "kowal@gmail.com", DateTime.Parse("2005-05-05"), 51);

        // Asert
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_isEmailValid()
    {
        // Arrange
        var UserService = new UserService();

        // Act
        var result = UserService.AddUser("Jan", "Kowalski", "kowalatgmaildotcom", DateTime.Parse("2000-05-05"), 51);

        // Asert
        Assert.False(result);
    }

    
}