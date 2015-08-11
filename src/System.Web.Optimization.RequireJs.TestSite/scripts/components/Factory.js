define(["knockout", "underscore"], function(ko, _) {
    return  {
        Day: function(name) {
            var that = this;
            that.Name = name;

            that.GetTasksForDay = function (tasks) {
                return _.filter(tasks, function (task) {
                    return task.Day === that.Name;
                });
            };
        },
        Shift: function(name, id) {
            var that = this;
            that.Name = name === "" ? "&nbsp;" : name;
            that.Id = id;
            that.rowHeight = ko.observable();

            that.GetTasksForShift = function(tasks) {
                return _.filter(tasks, function(task) {
                    return task.ShiftId === that.Id;
                });
            }
        },
        Task: function(name, shiftId, day) {
            var that = this;
            that.Name = name;
            that.ShiftId = shiftId;
            that.Day = day;
        }
    }
});