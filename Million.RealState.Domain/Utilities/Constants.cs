using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Million.RealState.Domain.Utilities;

public class Constants
{
    public const string Dbconnection = "SqlDB:ConnectionString";

    //Settings Token
    public const string JWT = "jwt:Issuer";
    public const string JWTAudience = "jwt:Audience";
    public const string JWTKey = "jwt:Key";

    //Cors
    public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    //Auth
    public const string Bearer = "Bearer";
    public const string Authorization = "Authorization";

    //JwtAuthService
    public const string InvalidToken = "Invalid token";
    public const string JwtAccessTokenExpiration = "Jwt:AccessTokenExpiration";

    public const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{5}$";
    public const string UserPattern = @"\bMillion\b";

    //validaciones 
    public const string Invalid = "Request should be valid";
    public const string RequestNull = "Request cannot be null";
    public const string ErrorUpdatingProperty = "An error occurred while updating the property";
    public const string ErrorCreatingProperty = "An error occurred while creating the property";
    public const string ErrorGettingProperties = "An error occurred while getting the properties";
    public const string ErrorAuthenticating = "An error occurred while authenticating";
    public const string ErrorCreatingImages = "An error occurred while creating the images";
    public const string ValidError = "Validation error";
    public const string InvalidRequest = "Invalid request";
    public const string SavedProperty = "Property saved successfully";
    public const string IdRequired = "The id is required";
    public const string NotFound = "Not found";
    public const string Success = "Successfuly";
    public const string AuthFailure = "Authentication failure";    
    public const string SavedImages = "Images of property saved successfully";
    public const string UpdatedPrice = "Price updated successfully";
    public const string TokenError = "Error token";
    public const string UserPasswordRequired = "\"Username and password are required";
    public const string RequiredOwnerId = "OwnerId is required";
    public const string ListImagesNoEmpty = "The list of images cannot be empty";
    public const string PriceNoZero = "The price should be greater than zero";
    public const string ErrorFilters = "Error retrieving properties with the given filters";
    public const string NoFoundWithFilters = "No properties found with the given filters";
}
