using Xunit;
using Api.Models;

public class ClientModelTests
{
     //////////////
    /// NAMING ///  MethodName_StateUnderTest_ExpectedReuls
    //////////////
    

    // Test methods must always return
    // void disregarding the actual method return type
    
	[Fact]
	public void Client_Defaults_AreEmptyStrings()
	{
		// Check the constructor
		var client = new Client();

		// ASSERT
		Assert.Equal(0, client.ClientId);
		Assert.Equal(string.Empty, client.FirstName);
		Assert.Equal(string.Empty, client.LastName);
		Assert.Equal(string.Empty, client.Email);
		Assert.Equal(string.Empty, client.Phone);
	}

	[Fact]
	public void Client_CanSetProperties()
	{
		// Check the constructor
		var client = new Client();

        // ACTUAL TESTING METHOD
		client.ClientId = 42;
		client.FirstName = "Ada";
		client.LastName = "Lovelace";
		client.Email = "ada@lovelace.org";
		client.Phone = "555-1234";

		// ASSERT
		Assert.Equal(42, client.ClientId);
		Assert.Equal("Ada", client.FirstName);
		Assert.Equal("Lovelace", client.LastName);
		Assert.Equal("ada@lovelace.org", client.Email);
		Assert.Equal("555-1234", client.Phone);
	}
}
