using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WPF_MVVM_TEMPLATE.Application.Utility;

public class ValidationManager : INotifyDataErrorInfo
{
    // INotifyDataErrorInfo expects us to hold WHAT is breaking, and the errors.
    private readonly Dictionary<string, List<string>> _errors = new();
    
    //Unique list of fields / elements we are concerned about.
    private readonly HashSet<string> _registeredFields = new(); // Fields registered for validation

    
    //Do we have any errors? Usefull to check if the list is clear.
    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>
    /// Register a single Field.
    /// </summary>
    /// <param name="fieldNames"></param>
    public void RegisterField(string fieldName)
    {
        if (!_registeredFields.Contains(fieldName))
            _registeredFields.Add(fieldName);
    }

    /// <summary>
    /// Register Multiple Fields.
    /// </summary>
    /// <param name="fieldNames"></param>
    public void RegisterFields(IEnumerable<string> fieldNames)
    {
        foreach (var fieldName in fieldNames)
        {
            RegisterField(fieldName);
        }
    }

    /// <summary>
    /// Adds a error so we can keep track of what is broken and why.
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="errorMessage"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddError(string fieldName, string errorMessage)
    {
        if (!_registeredFields.Contains(fieldName))
            throw new InvalidOperationException($"Field {fieldName} is not registered for validation.");

        if (!_errors.ContainsKey(fieldName))
            _errors[fieldName] = new List<string>();

        if (!_errors[fieldName].Contains(errorMessage))
        {
            _errors[fieldName].Add(errorMessage);
            OnErrorsChanged(fieldName);
        }
    }

    /// <summary>
    /// Clearing errors, Removing the relevant field from the error list
    /// </summary>
    /// <param name="fieldName"></param>
    public void ClearErrors(string fieldName)
    {
        if (_errors.Remove(fieldName))
            OnErrorsChanged(fieldName);
    }

    /// <summary>
    /// Gets all errors for one element
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public IEnumerable GetErrors(string propertyName)
    {
        return _errors.ContainsKey(propertyName) ? _errors[propertyName] : Enumerable.Empty<string>();    
    }
    
    /// <summary>
    /// Gets all errors as a dictionary of field names and their corresponding error messages.
    /// </summary>
    public IReadOnlyDictionary<string, List<string>> AllErrors
    {
        get
        {
            return _errors; // Return the dictionary of all errors
        }
    }


    /// <summary>
    /// Gets all errors as a single flattened list of error messages.
    /// Each entry contains the field name and the associated error message.
    /// </summary>
    public IEnumerable<string> AllErrorsFlattened
    {
        get
        {
            return _errors.SelectMany(kvp =>
            {
                // For each key-value pair (kvp), format errors as "FieldName: ErrorMessage"
                return kvp.Value.Select(error => $"{kvp.Key}: {error}");
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fieldName"></param>
    private void OnErrorsChanged(string fieldName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(fieldName));
    }
}