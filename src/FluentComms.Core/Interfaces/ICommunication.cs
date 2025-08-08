using FluentComms.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    /// <summary>
    /// Defines the fluent interface for building and sending a communication message.
    /// </summary>
    public interface ICommunication
    {
        /// <summary>
        /// Gets the underlying communication message being built.
        /// </summary>
        CommunicationMessage Message { get; }

        /// <summary>
        /// Sets the sender's address.
        /// </summary>
        /// <param name="address">The sender's email address or phone number.</param>
        /// <param name="name">The sender's display name.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication From(string address, string name = null);

        /// <summary>
        /// Adds a primary recipient to the message.
        /// </summary>
        /// <param name="address">The recipient's email address or phone number.</param>
        /// <param name="name">The recipient's display name.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication To(string address, string name = null);

        /// <summary>
        /// Adds a carbon copy (CC) recipient to the message.
        /// </summary>
        /// <param name="address">The CC recipient's email address.</param>
        /// <param name="name">The CC recipient's display name.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication Cc(string address, string name = null);

        /// <summary>
        /// Adds a blind carbon copy (BCC) recipient to the message.
        /// </summary>
        /// <param name="address">The BCC recipient's email address.</param>
        /// <param name="name">The BCC recipient's display name.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication Bcc(string address, string name = null);

        /// <summary>
        /// Sets the subject of the message.
        /// </summary>
        /// <param name="subject">The message subject.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication Subject(string subject);

        /// <summary>
        /// Sets the body of the message.
        /// </summary>
        /// <param name="body">The message body content.</param>
        /// <param name="isHtml">A flag indicating if the body is HTML.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication Body(string body, bool isHtml = false);

        /// <summary>
        /// Sets the body of the message using a template file and model.
        /// The template content will be read from the specified file path.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="filePath">The path to the template file.</param>
        /// <param name="model">The model containing the data for the template.</param>
        /// <param name="isHtml">A flag indicating if the template produces HTML content.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication UsingTemplateFromFile<T>(string filePath, T model, bool isHtml = false);

        /// <summary>
        /// Sets the body of the message using a template string and model.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="template">The template string.</param>
        /// <param name="model">The model containing the data for the template.</param>
        /// <param name="isHtml">A flag indicating if the template produces HTML content.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication UsingTemplate<T>(string template, T model, bool isHtml = false);

        /// <summary>
        /// Adds an attachment to the message.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <param name="data">A stream containing the file data.</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication Attach(string filename, Stream data, string contentType);

        /// <summary>
        /// Adds an attachment to the message from a file path.
        /// </summary>
        /// <param name="filePath">The full path to the file to attach.</param>
        /// <param name="contentType">The MIME content type of the file. If null, it will be inferred.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication Attach(string filePath, string contentType = null);

        /// <summary>
        /// Sets the priority of the message.
        /// </summary>
        /// <param name="priority">The message priority.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication WithPriority(Priority priority);

        /// <summary>
        /// Adds a provider-specific tag or metadata to the message.
        /// </summary>
        /// <param name="key">The key of the tag.</param>
        /// <param name="value">The value of the tag.</param>
        /// <returns>The fluent communication instance.</returns>
        ICommunication WithTag(string key, object value);

        /// <summary>
        /// Sends the message. This is the terminal method of the fluent chain.
        /// </summary>
        /// <returns>A task that represents the asynchronous send operation. The task result contains the provider's response.</returns>
        Task<SendResponse> SendAsync();
    }
}
