namespace SharedKernel.Constants;

public static class DomainModelConstants
{
    public const int GUID_STRING_ID_MAX_LENGTH = 36;
    public const int INDEX_STRING_MAX_LENGTH = 450;

    #region web
    public const int SEO_TITLE_MAX_LENGTH = 100; 
    public const int SEO_DESC_MAX_LENGTH = 400;
    public const int SEO_KEYWORDS_MAX_LENGTH = 256;
    public const int SEO_IMAGE_ALT_MAX_LENGTH = 125;
    public const int SEO_IMAGE_DESC_MAX_LENGTH = 250;
    public const string SEO_URL_SLUG_REGEX = @"^[a-z0-9]+(?:(?:-|\/)[a-z0-9]+)*$";
    #endregion

    #region value objects
    public const int NAME_MAX_LENGTH = 64;
    public const int ADDRESS_LINE_MAX_LENGTH = 128;
    public const int CITY_MAX_LENGTH = 100;
    public const int ZIPCODE_MAX_LENGTH = 18;
    public const int EMAIL_MIN_LENGTH = 6;
    public const int EMAIL_MAX_LENGTH = 256;
    public const int PHONE_NUMBER_MAX_LENGTH = 15;
    public const string PHONE_NUMBER_REGEX = @"^\+?(?:[0-9][ -]?){6,14}[0-9]$";
    public const string EMAIL_REGEX = @"^(?=.{6,256}$)(?=.{1,64}@.{4,254}$)" + 
        @"[a-z0-9]+(?:([.+]?|(_|-)+)[a-z0-9]+)*@[a-z](?:[-a-z]+\.)+[a-z]{2,6}$";
    #endregion

    #region country
    public const int COUNTRY_NAME_MAX_LENGTH = 64;
    #endregion

    #region catalog
    public const int CATEGORY_NAME_MAX_LENGTH = 128;
    public const int PRODUCT_SKU_LENGTH = 9;
    public const int PRODUCT_ID_LENGTH = 32;
    public const int PRODUCT_NAME_MAX_LENGTH = 128;
    public const int PRODUCT_DESC_MAX_LENGTH = 2048;
    public const string IMAGE_ALLOWED__EXT = ".png .jpg .jpeg .png .svg .webp";
    public const string VIDEO_ALLOWED__EXT = ".mp3 .mp4 .mov .webm";
    public const string DOCUMENT_ALLOWED_EXT = ".pdf .csv .doc .docx .xlsx";
    #endregion

    #region order
    public const int ORDER_TRACKING_NO_LENGTH = 8;
    public const int CARRIER_NAME_MAX_LENGTH = 64;
    #endregion

    #region mediaFile
    public const long MEDIA_FILE_MAX_SIZE = 1024 * 1024 * 30;
    #endregion

    #region tag
    public const int TAG_NAME_MAX_LENGTH = 64;
    #endregion
}
