tw.Gantt = function (data, options) {
    var self = this;

    //privates
    var total_duration = [];
    var total = tw.bindings.vm.totalTime();
    var width = 0;

    $.each(data, function (i, e) {
        e.RealAcc = getParentDuration(e, data);
    });

    var _default = {
        //draw area default config
        graphic_width: $("#chart").width(),
        graphic_height: 100,
        graphic_start_x: 0,
        graphic_start_y: 15,
        //item default config
        item_unit_width: 10,
        item_estimated_height: 15,
        item_real_height: 3,
        item_padding_x: 1,
        item_padding_y: 3,

        //item initial offset
        item_offset_x: 75,
        item_offset_y: 3,

        //grid setup
        grid_vertical_lines: 20,
        grid_horizontal_lines: data.length + 1,
        grid_header_offset: 13
    };

    //configuration
    self.options = $.extend(true, _default, options);

    self.item_unit_width = self.options.item_unit_width;
    self.item_estimated_height = self.options.item_estimated_height;
    self.item_real_height = self.options.item_real_height;
    self.item_padding_x = self.options.item_padding_x;
    self.item_padding_y = self.options.item_padding_y;

    self.item_offset_x = self.options.item_offset_x;
    self.item_offset_y = self.options.item_offset_y;

    self.grid_vertical_lines = self.options.grid_vertical_lines;
    self.grid_horizontal_lines = self.options.grid_horizontal_lines;
    self.grid_header_offset = self.options.grid_header_offset;

    self.graphic_width = self.options.graphic_width;
    self.graphic_start_x = self.options.graphic_start_x;
    self.graphic_start_y = self.options.graphic_start_y;
    self.graphic_height = (data.length * self.item_estimated_height) + self.grid_header_offset + ((data.length - 2) * self.item_offset_y);


    self.data = data;
    width = self.graphic_width * .97;
    self.item_unit_width = (width - self.item_offset_x - self.item_padding_x) / total;

    //d3 initialization
    var layout = d3.layout.pack()
        .size([self.graphic_width, self.graphic_height]);

    var graphic = d3.select("#chart").append("svg")
        .attr("width", self.graphic_width)
        .attr("height", self.graphic_height + self.graphic_start_y);

    //drawing the boxes
    drawChart(graphic);

    //drawing the chart axis
    drawGrid(graphic);

    function drawChart(g) {

        var node = g.selectAll("rect")
            .data(self.data)
            .enter();

        //item name
        node.append("text")
            .text(function (d) {
                return d.Name;
            }).attr("x", 5)
            .attr("y", function (d, i) {
                return self.grid_header_offset + self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i;
            })
            .attr("class", "gantt_text");

        //item estimated duration bar
        node.append("rect")
            .attr("x", function (d, i) {
                return (i && self.item_padding_x) + self.item_offset_x + (Math.max(d.AccumulatedTime, d.RealAcc) * self.item_unit_width);
            }).attr("y", function (d, i) {
                return self.grid_header_offset + self.item_offset_y + (i) * (self.item_padding_y + self.item_estimated_height);
            }).attr("width", function (d, i) {
                var noDuration = (total * self.item_unit_width) - ((i && self.item_padding_x) + self.item_offset_x + (Math.max(d.AccumulatedTime, d.RealAcc) * self.item_unit_width));
                return (d.Duration || (total - Math.max(d.AccumulatedTime, d.RealAcc))) * self.item_unit_width;
            }).attr("height", self.item_estimated_height)
            .attr("class", "gantt_duration_rect")
            .on("mouseover", function (d, i) { tooltipMousover(d, i); })
            .on("mouseout", function (d, i) { tooltipMouseout(d); });


        //item real duration bar
        node.append("rect")
            .attr("x", function (d, i) {
                return (i && self.item_padding_x) + self.item_offset_x + (Math.max(d.AccumulatedTime, d.RealAcc) * self.item_unit_width);
            }).attr("y", function (d, i) {
                return self.grid_header_offset + self.item_offset_y + ((self.item_padding_y + self.item_estimated_height) * i) + (self.item_estimated_height - self.item_real_height);
            }).attr("width", function (d) {
                return d.TimeUsed * self.item_unit_width;
            }).attr("height", self.item_real_height)
            .attr("class", function (d) {
                if (d.TimeUsed >= d.Duration * 0.95)
                    return "gantt_duration_rect_red";
                if (d.TimeUsed >= d.Duration * 0.65)
                    return "gantt_duration_rect_yellow";
                return "gantt_duration_rect_green";
            })
            .on("mouseover", function (d, i) { tooltipMousover(d, i); })
            .on("mouseout", function (d, i) { tooltipMouseout(d); });

        //item duration text
        node.append("text")
            .text(function (d) {
                return (d.Duration || "\u221E") + "h/" + d.TimeUsed + "h (" + (d.Duration && ((d.TimeUsed / d.Duration) * 100).toPrecision(3)) + "%)";
            }).attr("x", function (d, i) {
                return (i && self.item_padding_x) + self.item_offset_x + (Math.max(d.AccumulatedTime, d.RealAcc) * self.item_unit_width);
            }).attr("y", function (d, i) {
                return self.grid_header_offset + self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i - 2;
            }).attr("dx", function (d) {
                return ((d.Duration || (total - Math.max(d.AccumulatedTime, d.RealAcc))) * self.item_unit_width) / 2;
            })
            .attr("class", "gantt_rect_text");

        //tooltip
        var tooltip = $("#gantt_tooltip");
        tooltip.popover();

        function tooltipMousover(item, index) {
            tw.bindings.vm.tooltip.map(item);
            if ($(item).data('popover') == undefined)
                $(item).popover({ trigger: "hover", content: $(tooltip).children("#tooltip_body").html(), title: $(tooltip).children("#tooltip_title").text() });

            $(item).popover("show");

            var elem = $("#chart svg rect.gantt_duration_rect:eq(" + index + ")"),
                position = elem.position(),
                popover = $(".popover");
            position.left += parseFloat(elem.attr("width"));
            position.top -= $(".popover-inner").height() / 2;
            popover.offset(position);


        }

        function tooltipMouseout(item) {
            $(item).popover("hide");
        }
    }

    function drawGrid(g) {

        var x = d3.scale.linear()
            .domain([0, total])
            .range([self.item_offset_x, width]);

        g.selectAll("linev")
            .data(x.ticks(self.grid_vertical_lines))
            .enter().insert("line", ":first-child")
            .attr("x1", x).attr("y1", Math.floor(self.grid_header_offset) + 1)
            .attr("x2", x).attr("y2", Math.floor(self.graphic_height + self.grid_header_offset / 2) + 2)
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
            .data(d3.range(0, self.grid_horizontal_lines))
            .enter().insert("line", ":first-child")
            .attr("x1", 0)
            .attr("y1", function (d, i) {
                return (self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i) - 0.5;
            })
            .attr("x2", function (d, i) {
                return self.graphic_width;
            })
            .attr("y2", function (d, i) {
                return (self.graphic_start_y + (self.item_padding_y + self.item_estimated_height) * i) - 0.5;
            })
            .attr("width", 1)
            .style("stroke", "#ccc");

        g.selectAll("linex")
            .data([1])
            .enter().insert("line", ":first-child")
            .attr("x1", 1).attr("y1", Math.floor(self.grid_header_offset) + 1)
            .attr("x2", 1).attr("y2", Math.floor(self.graphic_height + self.grid_header_offset / 2) + 2)
            .attr("width", 1)
            .style("stroke", "#ccc");


        g.selectAll("header_leg_bottom")
            .data([1])
            .enter().append("text")
            .attr("x", 2)
            .attr("y", 10)
            .attr("dy", 0)
            .attr("text-anchor", "start")
            .text("Activity \\ Time")
            .attr("font-size", "0.7em");
    }

    function getParentDuration(a, _data, _acc) {
        var acc = _acc || 0;

        var max = 0;
        var parents = $.grep(_data, function (element, index) {
            return $.inArray(element.Id, a.Dependencies) > -1;
        });

        if (parents.length == 0)
            return acc;

        var parent = undefined;
        var ref = max;
        parents.forEach(function (e, i) {
            max = Math.max(Math.max(e.Duration, e.TimeUsed), max);
            if (max != ref)
                parent = e;
        });

        return getParentDuration(parent, _data, acc + max);
    }

}