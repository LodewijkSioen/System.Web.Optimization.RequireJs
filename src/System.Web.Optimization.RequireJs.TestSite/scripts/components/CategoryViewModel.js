define(["knockout", "text!components/CategoryViewModel.html"], function (ko, htmlTemplate) {
    var viewModel = function (params) {
        var that = this;
        that.Tasks = ko.observableArray(params.Tasks);
    };

    return { viewModel: viewModel, template: htmlTemplate };
});