﻿using System;
using System.Collections.Generic;
using System.Security;

namespace Mastersign.Bench
{
    internal delegate bool PropertyCriteria(string name);

    internal delegate bool GroupedPropertyCriteria(string group, string name);

    internal delegate string PropertyBasePathSource(string name);

    internal delegate string GroupedPropertyBasePathSource(string group, string name);

    internal delegate BenchUserInfo UserInfoSource(string prompt);

    internal delegate SecureString PasswordSource(string prompt);

    internal delegate void TextFileEditor(string prompt, string filePath);

    /// <summary>
    /// The type for a handler, called when the download of string finished.
    /// </summary>
    /// <param name="success"><c>true</c> if the download was successful; otherwise false.</param>
    /// <param name="content">The content of the download or <c>null</c> if the download failed.</param>
    public delegate void StringDownloadResultHandler(bool success, string content);

    /// <summary>
    /// The type for a handler, called when the download of a file finished.
    /// </summary>
    /// <param name="success"><c>true</c> if the download was successfull; otherwise false.</param>
    public delegate void FileDownloadResultHandler(bool success);

    /// <summary>
    /// The type for a method which is called, to process a key value pair of strings.
    /// </summary>
    /// <param name="key">The key to process.</param>
    /// <param name="value">The value to process.</param>
    public delegate void DictionaryEntryHandler(string key, string value);

    /// <summary>
    /// The type for a method which is called when a process finished its execution.
    /// </summary>
    /// <param name="result">The return code, and possible the output of the process.</param>
    public delegate void ProcessExitCallback(ProcessExecutionResult result);

    internal delegate void BenchTask(IBenchManager man, ICollection<AppFacade> apps,
        Action<TaskInfo> notify, Cancelation cancelation);

    /// <summary>
    /// The type of a method, executing a Bench task for all Bench apps.
    /// </summary>
    /// <param name="man">The Bench manager.</param>
    /// <param name="notify">The notification handler.</param>
    /// <param name="cancelation">A cancelation token.</param>
    /// <returns>The result of the executed task in the shape of an <see cref="ActionResult"/> object.</returns>
    public delegate ActionResult BenchTaskForAll(IBenchManager man,
        Action<TaskInfo> notify, Cancelation cancelation);

    /// <summary>
    /// The type of a method, executing a Bench task for one Bench app.
    /// </summary>
    /// <param name="man">The Bench manager.</param>
    /// <param name="appId">The ID of the targeted app.</param>
    /// <param name="notify">The notification handler.</param>
    /// <param name="cancelation">A cancelation token.</param>
    /// <returns>The result of the executed task in the shape of an <see cref="ActionResult"/> object.</returns>
    public delegate ActionResult BenchTaskForOne(IBenchManager man, string appId,
        Action<TaskInfo> notify, Cancelation cancelation);
}