﻿namespace Noodle_Assignment.Interface
{
    public interface ICreateService
    {
        Task<string> ExecuteAsync(CustomerDraft draft);
    }
}
