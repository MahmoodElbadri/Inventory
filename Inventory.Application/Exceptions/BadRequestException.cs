using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Exceptions;

public class BadRequestException(string message): Exception(message)
{

}
