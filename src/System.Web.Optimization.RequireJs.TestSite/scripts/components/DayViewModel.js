define(["knockout", "text!components/DayViewModel.html"], function (ko, htmlTemplate) {
    var viewModel = function (params) {
        var that = this;
        that.Name = params.Name;
        that.Tasks = params.tasks;
    };

    return { viewModel: viewModel, template: htmlTemplate };
});