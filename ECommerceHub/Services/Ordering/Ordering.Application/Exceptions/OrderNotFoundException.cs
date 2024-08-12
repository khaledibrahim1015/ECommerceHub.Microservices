﻿namespace Ordering.Application.Exceptions;

public  class OrderNotFoundException :ApplicationException
{

    public OrderNotFoundException(string name , object key ) :base($"Entity {name} - key{key} is not found ")
    {
    }
}
