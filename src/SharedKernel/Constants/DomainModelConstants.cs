﻿namespace SharedKernel.Constants;

public static class DomainModelConstants
{
    public const int INDEX_STRING_MAX_LENGTH = 450;
    #region web
    public const int SEO_TITLE_MAX_LENGTH = 100; 
    public const int SEO_DESC_MAX_LENGTH = 400;
    public const string SEO_URL_SLUG_REGEX = @"^[a-z]+(?:(\/|-)?[a-z]+)*$";
    #endregion

    #region value objects
    public const int NAME_MAX_LENGTH = 64;
    public const int ADDRESS_LINE_MAX_LENGTH = 128;
    public const int CITY_MAX_LENGTH = 100;
    public const int ZIPCODE_MAX_LENGTH = 18;
    public const int COUNTRY_MAX_LENGTH = 64;
    public const int EMAIL_MIN_LENGTH = 6;
    public const int EMAIL_MAX_LENGTH = 256;
    public const int PHONE_NUMBER_MAX_LENGTH = 13;
    public const string PHONE_NUMBER_REGEX = @"^\+?\d{7,15}$";
    public const string EMAIL_REGEX = @"^(?=.{6,256}$)(?=.{1,64}@.{4,254}$)" + 
        @"[a-z0-9]+(?:([.+]?|(_|-)+)[a-z0-9]+)*@[a-z](?:[-a-z]+\.)+[a-z]{2,6}$";
    #endregion

    #region catalog
    public const int CATEGORY_NAME_MAX_LENGTH = 128;
    public const int PRODUCT_SKU_LENGTH = 9;
    public const int PRODUCT_NAME_MAX_LENGTH = 128;
    public const int PRODUCT_DESC_MAX_LENGTH = 2048;
    #endregion

    #region order
    public const int ORDER_TRACKING_NO_LENGTH = 40;
    public const int CARRIER_NAME_MAX_LENGTH = 64;
    #endregion
}
