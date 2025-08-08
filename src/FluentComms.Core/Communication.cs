using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FluentComms.Core
{
    /// <summary>
    /// The default implementation of the ICommunication fluent interface.
    /// </summary>
    internal class Communication : ICommunication
    {
        private readonly ISender _sender;
        private readonly IRenderer _renderer;

        /// <summary>
        /// Gets the underlying communication message being built.
        /// </summary>
        public CommunicationMessage Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Communication"/> class.
        /// </summary>
        /// <param name="sender">The sender to be used for dispatching the message.</param>
        /// <param name="renderer">The renderer for parsing templates.</param>
        public Communication(ISender sender, IRenderer renderer)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            Message = new CommunicationMessage();
        }

        /// <inheritdoc />
        public ICommunication From(string address, string name = null)
        {
            Message.FromAddress = new Address(address, name);
            return this;
        }

        /// <inheritdoc />
        public ICommunication To(string address, string name = null)
        {
            Message.ToAddresses.Add(new Address(address, name));
            return this;
        }

        /// <inheritdoc />
        public ICommunication Cc(string address, string name = null)
        {
            Message.CcAddresses.Add(new Address(address, name));
            return this;
        }

        /// <inheritdoc />
        public ICommunication Bcc(string address, string name = null)
        {
            Message.BccAddresses.Add(new Address(address, name));
            return this;
        }

        /// <inheritdoc />
        public ICommunication Subject(string subject)
        {
            Message.Subject = subject;
            return this;
        }

        /// <inheritdoc />
        public ICommunication Body(string body, bool isHtml = false)
        {
            Message.Body = body;
            Message.IsHtml = isHtml;
            Message.TemplateModel = null; // Clear template if body is set directly
            return this;
        }

        /// <inheritdoc />
        public ICommunication UsingTemplateFromFile<T>(string filePath, T model, bool isHtml = false)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified template file could not be found.", filePath);

            var template = File.ReadAllText(filePath);
            return UsingTemplate(template, model, isHtml);
        }

        /// <inheritdoc />
        public ICommunication UsingTemplate<T>(string template, T model, bool isHtml = false)
        {
            Message.Body = template;
            Message.TemplateModel = model;
            Message.IsHtml = isHtml;
            return this;
        }

        /// <inheritdoc />
        public ICommunication Attach(string filename, Stream data, string contentType)
        {
            Message.Attachments.Add(new Attachment { Filename = filename, Data = data, ContentType = contentType });
            return this;
        }

        /// <inheritdoc />
        public ICommunication Attach(string filePath, string contentType = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified attachment file could not be found.", filePath);

            var fileInfo = new FileInfo(filePath);
            var stream = fileInfo.OpenRead();

            // Basic MIME type inference if not provided
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "application/octet-stream"; // Default
            }

            return Attach(fileInfo.Name, stream, contentType);
        }

        /// <inheritdoc />
        public ICommunication WithPriority(Priority priority)
        {
            Message.Priority = priority;
            return this;
        }

        /// <inheritdoc />
        public ICommunication WithTag(string key, object value)
        {
            Message.Tags[key] = value;
            return this;
        }

        /// <inheritdoc />
        public async Task<SendResponse> SendAsync()
        {
            // If a template model is present, render the body before sending
            if (Message.TemplateModel != null)
            {
                Message.Body = await _renderer.ParseAsync(Message.Body, Message.TemplateModel, Message.IsHtml).ConfigureAwait(false);
            }

            return await _sender.SendAsync(Message, default).ConfigureAwait(false);
        }
    }
}
