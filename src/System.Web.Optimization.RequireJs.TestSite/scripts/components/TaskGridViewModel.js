define(["knockout", "text!components/TaskGridViewModel.html"], function (ko, htmlTemplate) {
    var viewModel = function(params) {
        var that = this;
        that.Shifts = params.shifts;
        that.Days = params.days;
        that.Tasks = params.tasks;
    };
    
    return { viewModel: viewModel, template: htmlTemplate };
});