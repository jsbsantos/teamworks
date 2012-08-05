
TW.graphics.Gantt = function (data, options) {
    var self = this;

    //privates
    var total_duration = [];
    var total = 0;
    var width = 0;

    for (var e in data) {
        var d = Math.max(data[e].Duration, data[e].TimeUsed);
        if (total_duration[data[e].StartDate] == undefined) {
            total_duration[data[e].StartDate] = d;
            total += d;
        } else if (d > total_duration[data[e].StartDate]) {
            total += d - total_duration[data[e].StartDate];
        }
    }

    var _default = {
        //draw area default config
        graphic_width: $("#chart").width(),
        graphic_height: 150,
        graphic_start_x: 0,
        graphic_start_y: 15,
        //item default config
        item_unit_width: 10,
        item_estimated_height: 15,
        item_real_height: 4,
        item_padding_x: 1,
        item_padding_y: 3,

        //item initial offset
        item_offset_x: 50,
        item_offset_y: 3,

        //grid setup
        grid_vertical_lines: 20,
        grid_horizontal_lines: data.length + 1,
        grid_header_offset: 13
    };

    //configuration
    self.options = $.extend(true, _default, options);

    self.graphic_width = self.options.graphic_width || self.graphic_width || 0;
    self.graphic_height = self.options.graphic_height || self.graphic_height || 0;
    self.graphic_start_x = self.options.graphic_start_x || self.graphic_start_x || 0;
    self.graphic_start_y = self.options.graphic_start_y || self.graphic_start_y || 0;

    self.item_unit_width = self.options.item_unit_width || self.graphic_width || 0;
    self.item_estimated_height = self.options.item_estimated_height || self.item_estimated_height || 0;
    self.item_real_height = self.options.item_real_height || self.item_real_height || 0;
    self.item_padding_x = self.options.item_padding_x || self.item_padding_x || 0;
    self.item_padding_y = self.options.item_padding_y || self.item_padding_y || 0;

    self.item_offset_x = self.options.item_offset_x || self.item_offset_x || 0;
    self.item_offset_y = self.options.item_offset_y || self.item_offset_y || 0;

    self.grid_vertical_lines = self.options.grid_vertical_lines || self.grid_vertical_lines || 0;
    self.grid_horizontal_lines = self.options.grid_horizontal_lines || self.grid_horizontal_lines || 0;
    self.grid_header_offset = self.options.grid_header_offset || self.grid_header_offset || 0;

    self.data = data;
    width = self.graphic_width * .98;
    self.item_unit_width = (width - self.item_offset_x - self.item_padding_x) / total;

    //d3 initialization
    var layout = d3.layout.pack()
        .size([self.graphic_width, self.graphic_height]);

    var graphic = d3.select("#chart").append("svg")
        .attr("width", self.graphic_width)
        .attr("height", self.graphic_height + self.graphic_start_y);

    //drawing the boxes
    DrawChart(graphic);

    //drawing the chart axis
    DrawGrid(graphic);

    function DrawChart(g) {

        var node = g.selectAll("rect")
            .data(self.data)
            .enter();

        //item name
        node.append("text")
            .text(function (d, i) {
                return d.Name;
            }).attr("x", function (d, i) {
                return 0;
            }).attr("y", function (d, i) {
                return self.grid_header_offset + self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i;
            })
            .attr("fill", "black")
            .attr("font-size", "0.9em");

        //item estimated duration bar
        node.append("rect")
            .attr("x", function (d, i) {
                return (i && self.item_padding_x) + self.item_offset_x + (d.AccumulatedTime * self.item_unit_width);
            }).attr("y", function (d, i) {
                return self.grid_header_offset + self.item_offset_y + (i) * (self.item_padding_y + self.item_estimated_height);
            }).attr("width", function (d, i) {
                return d.Duration * self.item_unit_width;
            }).attr("height", self.item_estimated_height)
            .attr("rx", 4)
            .attr("ry", 4)
            .style("fill", "#08C")
            .style("stroke", "rgb(0,0,0)")
            .style("stroke-width", 1)
            .style("stroke-opacity", 0.5);

        //item real duration bar
        node.append("rect")
            .attr("x", function (d, i) {
                return (i && self.item_padding_x) + self.item_offset_x + (d.AccumulatedTime * self.item_unit_width);
            }).attr("y", function (d, i) {
                return self.grid_header_offset + self.item_offset_y + ((self.item_padding_y + self.item_estimated_height) * i) + (self.item_estimated_height - self.item_real_height - 1);
            }).attr("width", function (d, i) {
                return d.TimeUsed * self.item_unit_width;
            }).attr("height", self.item_real_height)
            .attr("rx", 4)
            .attr("ry", 4)
            .style("fill", function (d, i) {
                if (d.TimeUsed >= d.Duration * 0.95)
                    return "red";
                if (d.TimeUsed >= d.Duration * 0.65)
                    return "yellow";
                return "green";
            });

        //item duration text
        node.append("text")
            .text(function (d, i) {
                return d.Duration + "h/" + d.TimeUsed + "h (" + (d.TimeUsed / d.Duration) * 100 + "%)";
            }).attr("x", function (d, i) {
                return (i && self.item_padding_x) + self.item_offset_x + (d.AccumulatedTime * self.item_unit_width);
            }).attr("y", function (d, i) {
                return self.grid_header_offset + self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i - 2;
            }).attr("dx", function (d, i) {
                return (d.Duration * self.item_unit_width) / 2;
            })
            .attr("fill", "black")
            .attr("font-size", "0.8em")
            .attr("text-anchor", "middle");
    }

    function DrawGrid(g) {

        var x = d3.scale.linear()
            .domain([0, total])
            .range([self.item_offset_x, width]);

        g.selectAll("linev")
            .data(x.ticks(self.grid_vertical_lines))
            .enter().insert("line", ":first-child")
            .attr("x1", x).attr("y1", Math.floor(self.grid_header_offset)+1)
            .attr("x2", x).attr("y2", Math.floor(self.graphic_height + self.grid_header_offset / 2)+2)
            .attr("width", 1)
            .style("stroke", "#ccc");

        g.selectAll("header_text")
            .data(x.ticks(self.grid_vertical_lines))
            .enter().insert("text", ":first-child")
            .attr("x", x)
            .attr("y", self.grid_header_offset)
            .attr("dy", -3)
            .attr("text-anchor", "middle")
            .text(function (d, i) { return d; })
            .attr("font-size", "0.8em");


        g.selectAll("lineh")
            .data(d3.range(0,19))
            .enter().insert("line", ":first-child")
            .attr("x1", 0)
            .attr("y1", function (d, i) {
                return (self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i)-0.5;
            })
            .attr("x2", function (d, i) {
                return self.graphic_width;
            })
            .attr("y2", function (d, i) {
                return (self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i)-0.5;
            })
            .attr("width", 1)
            .style("stroke", "#ccc");

        /*g.selectAll("linex")
        .data([1])
        .enter().insert("line", ":first-child")
        .attr("x1", 0)
        .attr("y1", self.item_padding_y)
        .attr("x2", self.graphic_start_x + self.item_padding_x)
        .attr("y2", self.graphic_start_y + self.item_offset_y)
        .attr("width", 1)
        .style("stroke", "#ccc");

        g.selectAll("header_leg_bottom")
        .data([1])
        .enter().append("text")
        .attr("x", 6)
        .attr("y", 14)
        .attr("dy", -3)
        .attr("text-anchor", "start")
        .text("Activity")
        .attr("font-size", "0.7em")
        .attr("transform", function (d) { return "rotate(28)"; });

        g.selectAll("header_leg_top")
        .data([1])
        .enter().append("text")
        .attr("x", 20)
        .attr("y", 9)
        .attr("text-anchor", "start")
        .text("Hours")
        .attr("font-size", "0.7em")
        .attr("transform", function (d) { return "rotate(28, 20,9)"; });*/
    }
}