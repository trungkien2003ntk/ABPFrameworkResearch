namespace MyDemo.Bookstore.Permissions;

public static class BookstorePermissions
{
    public const string GroupName = "Bookstore";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
