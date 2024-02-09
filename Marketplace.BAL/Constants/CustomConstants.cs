namespace Marketplace.BAL.Constants;
public static class CustomConstants
{
    public static class Error
    {
        public static string ImagesLimit = "The Product Can Have Five Images!";
        public static string NotExistImage = "The Main Image You Select Is Not In This Product Images, Please Add It First";
    }
    public static class SortOrder
    {
        public static string Descinding = "DESC";
    }
    public static class Operation
    {
        public static string Successful = "Successful Operation";
    }
    public static class User
    {
        public static string UserCreated = "User Created Successfuly";
        public static string UserLogin = "User Login Successfuly";
        public static string EmailIsTaken = "This Email Is Taken";
        public static string Incorrect = "Incorrect Email Or Password";
    }
    public static class NotFound
    {
        public static string Product = "Product Not Found!";
        public static string NoProducts = "No Products Found!";
        public static string Attribute = "Attribute Not Found!";
        public static string Variant = "Variant Not Found!";
    }
}
