using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Enums;

public enum ErrorCode
{
    RequestInvalid,
    Unauthorized,
    WrongPassword,
    EntityDoesNotExist,
    EntityAlreadyExists,
    IdentityError,
    InternalServerError,
    InvalidCredentials,
    EmptyFile,
    MaxFileSizeReached,
    CanOnlyUploadPhotos,
    WrongAspectRatio,
    MissingAspectRatio,
    EmailNotSent,
    InvalidGoogleAccount,
    AlreadyConfirmedEmail,
    InvalidConfirmationCode,
    ConfirmationCodeExpired,
    MustPassAtLeastOneSearchByProperty,
    OrderCannotBeMoreThanTotal,
    ErrorReorderingTask,
    ErrorSavingTasksFromLocalStorage,
    EmailIsRequiredForSocialLogins,
    UnknownSocialLogin,
    NoRoleAssigned,
    InvalidRole,
    UserWithSameEmailAlreadyExists,
    PasswordInvalid,
    CannotDeleteYourself,
    CannotDeleteGenericGroup,
    CannotRenameGenericGroup
}
