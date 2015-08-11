define(["knockout", "text!components/TaskViewModel.html", "jquery"], function(ko, htmlTemplate, $) {
    var viewModel = function (params) {
        var that = this;
        that.Name = params.task.Name || "";
        that.ShiftId = params.task.ShiftId;

        that.update = function (element, valueAccessor, allBindings, viewModel) {
            if (viewModel.ShiftId) {
                var height = $(element).closest("tr").height();
                $("#" + viewModel.ShiftId).height(height);
            }
        }
    };

    return { viewModel: viewModel, template: htmlTemplate };
});