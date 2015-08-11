define(["knockout", "text!components/ShiftViewModel.html", "underscore"], function (ko, htmlTemplate, _) {
    var viewModel = function (params) {
        var that = this;
        that.Name = params.name;
        that.Id = params.id;
        that.Days = params.days;
        that.Tasks = params.tasks;
    };

    return { viewModel: viewModel, template: htmlTemplate };
});