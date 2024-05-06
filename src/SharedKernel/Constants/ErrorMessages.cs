namespace SharedKernel.Constants;

public static class ErrorMessages
{
    public static class Common
    {
        public const string InvalidId = "Invalid Id type.";
        public const string NullOrEmptyId = "Id is null or empty";
        public const string DefaultDateTime = "Cannot have default date/time value";
        public const string ValidationError = "One or more validation failures have occurred.";
    }

    public static class FileEntry
    {
        public const string InvalidFileType = "Invalid file type option.";
    }

    public static class Product
    {
        public const string NotExist = "Product does not exist.";
        public const string DuplicateSku = "Product with sku exists.";
        public const string DuplicateName = "Product with name exists.";
        public const string InvalidVisibility = "Invalid visibility option.";
        public const string InvalidGiftCardType = "Invalid gift card option.";
        public const string InvalidSalePrice = "Sale price must be less than unit price.";
        public const string InvalidScheduleDate = "Scheduled date/time must be greater than 5 mins from current time";
    }

    public static class MailingList
    {
        public const string InvalidSubcribtionType = "Invalid subcribtion type.";
    }

    public static class Order
    {
        public const string NotExist = "Order does not exist.";
        public const string InvalidOrderStatus = "Invalid order status.";
        public const string InvalidCarrierOption = "Invalid carrier option.";
        public const string InvalidPaymentStatus = "Invalid payment status.";
        public const string NoCarrier = "Carrier not specified.";
    }
}
