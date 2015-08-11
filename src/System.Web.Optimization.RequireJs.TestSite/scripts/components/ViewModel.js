define(["knockout", "text!components/ViewModel.html"], function (ko, htmlTemplate) {
    var viewModel = function (params) {
        this.text = ko.observable(params.initialText);
        this.reset = function () {
            this.text(params.initialText);
        }
    };

    return { viewModel: viewModel, template: htmlTemplate };
});