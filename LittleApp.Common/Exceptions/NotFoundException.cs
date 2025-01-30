using LittleApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleApp.Common.Exceptions;

public class NotFoundException : SystemException
{
    public NotFoundException(ErrorCode errorCode, object responseParams = null) : base(errorCode, responseParams) { }

    public NotFoundException(List<ExceptionDetail> details) : base(details) { }
}
