using Rey.Hunter.Verification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc {
    public abstract class ReyController : Controller {
        protected IVerifier Verifier {
            get { return new Verifier(); }
        }

        #region Invoke

        protected IActionResult Invoke(Func<IActionResult> action, Func<Exception, IActionResult> error = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                return action();
            } catch (Exception ex) {
                return OnInvokeException(ex, error);
            }
        }

        protected async Task<IActionResult> InvokeAsync(Func<Task<IActionResult>> action, Func<Exception, IActionResult> error = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                return await action();
            } catch (Exception ex) {
                return OnInvokeException(ex, error);
            }
        }

        protected Task<IActionResult> InvokeAsync(Func<IActionResult> action, Func<Exception, IActionResult> error = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return Task.Run(() => this.Invoke(action, error));
        }

        protected virtual IActionResult OnInvokeException(Exception exception, Func<Exception, IActionResult> error) {
            if (error == null) {
                throw exception;
            }
            return error(exception);
        }

        #endregion //! Invoke

        #region JsonInvoke

        protected IActionResult JsonInvoke(Action action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                action();
                return GenreateJsonResult(GenerateSuccessJsonResultValue(), serializerSettings);
            } catch (Exception ex) {
                return GenreateJsonResult(GenerateFailedJsonResultValue(ex), serializerSettings);
            }
        }

        protected async Task<IActionResult> JsonInvokeAsync(Func<Task> action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                await action();
                return GenreateJsonResult(GenerateSuccessJsonResultValue(), serializerSettings);
            } catch (Exception ex) {
                return GenreateJsonResult(GenerateFailedJsonResultValue(ex), serializerSettings);
            }
        }

        protected Task<IActionResult> JsonInvokeAsync(Action action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return Task.Run(() => JsonInvoke(action, serializerSettings));
        }

        protected IActionResult JsonInvokeOne<T>(Func<T> action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                var model = action();
                return GenreateJsonResult(GenerateSuccessJsonResultValue(model), serializerSettings);
            } catch (Exception ex) {
                return GenreateJsonResult(GenerateFailedJsonResultValue(ex), serializerSettings);
            }
        }

        protected async Task<IActionResult> JsonInvokeOneAsync<T>(Func<Task<T>> action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                var model = await action();
                return GenreateJsonResult(GenerateSuccessJsonResultValue(model), serializerSettings);
            } catch (Exception ex) {
                return GenreateJsonResult(GenerateFailedJsonResultValue(ex), serializerSettings);
            }
        }

        protected Task<IActionResult> JsonInvokeOneAsync<T>(Func<T> action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return Task.Run(() => JsonInvokeOne(action, serializerSettings));
        }

        protected IActionResult JsonInvokeMany<T>(Func<IEnumerable<T>> action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                var models = action();
                return GenreateJsonResult(GenerateSuccessJsonResultValue(models), serializerSettings);
            } catch (Exception ex) {
                return GenreateJsonResult(GenerateFailedJsonResultValue(ex), serializerSettings);
            }
        }

        protected async Task<IActionResult> JsonInvokeManyAsync<T>(Func<Task<IEnumerable<T>>> action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try {
                var models = await action();
                return GenreateJsonResult(GenerateSuccessJsonResultValue(models), serializerSettings);
            } catch (Exception ex) {
                return GenreateJsonResult(GenerateFailedJsonResultValue(ex), serializerSettings);
            }
        }

        protected Task<IActionResult> JsonInvokeManyAsync<T>(Func<IEnumerable<T>> action, JsonSerializerSettings serializerSettings = null) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return Task.Run(() => JsonInvokeMany(action, serializerSettings));
        }

        protected virtual object GenerateJsonResultValue(int errCode, string errMsg) {
            return new { err_code = errCode, err_msg = errMsg };
        }

        protected virtual object GenerateJsonResultValue<T>(int errCode, string errMsg, T model) {
            return new { err_code = errCode, err_msg = errMsg, model = model };
        }

        protected virtual object GenerateJsonResultValue<T>(int errCode, string errMsg, IEnumerable<T> models) {
            return new { err_code = errCode, err_msg = errMsg, models = models };
        }

        protected virtual object GenerateSuccessJsonResultValue() {
            return GenerateJsonResultValue(0, "ok");
        }

        protected virtual object GenerateSuccessJsonResultValue<T>(T model) {
            return GenerateJsonResultValue(0, "ok", model);
        }

        protected virtual object GenerateSuccessJsonResultValue<T>(IEnumerable<T> models) {
            return GenerateJsonResultValue(0, "ok", models);
        }

        protected virtual object GenerateFailedJsonResultValue(Exception exception) {
            return GenerateJsonResultValue(-1, exception.Message);
        }

        protected virtual IActionResult GenreateJsonResult(object value, JsonSerializerSettings serializerSettings = null) {
            return serializerSettings == null ? new JsonResult(value) : new JsonResult(value, serializerSettings);
        }

        #endregion //! JsonInvoke
    }
}
