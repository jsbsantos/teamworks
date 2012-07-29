var TW = TW || { };
TW.Gantt = function (jsonendpoint) {
    //vertical_ticks - multiples of 5
    var vertical_ticks = 20, horizontal_ticks = 0/*also used as element count*/,
        chart_width = $("#chart").width() * 0.98/*width=100%*/, chart_height = 150,
        chart_start_padx = 55, chart_start_pady = 20, headeroffset = 10,
        padX = 0, padY = 2,
        box_width = 0.1, box_height = 13, boxoffset = -box_height - 0.5,
        total_duration = 0;

    var x = undefined; /*d3.scale.linear()
        .domain([0, total_duration || (chart_width - chart_start_padx) / 10])
        .range([chart_start_padx, chart_width]);*/

    var bubble = d3.layout.pack()
        .size(["100%", chart_height]);

    var vis = d3.select("#chart").append("svg")
        .attr("width", "100%")
        .attr("height", chart_height+chart_start_pady);

    //draw gantt chart bars
    d3.json(jsonendpoint, function (json) {
        var node = vis.selectAll("rect")
            .data(bubble.nodes(flatten(json))
                    .filter(function (d) { return !d.children; }))
            .enter();

        //item name
        node.append("text").text(function (d) {
            return d.tw_element.name;
        }).attr("x", function (d, i) {
            return 0;
        }).attr("y", function (d, i) {
            return d.tw_y + padY * i - 2; //2 -> line offset
        })
            .attr("fill", "black")
            .attr("font-size", "0.9em");

        //item duration box
        node.append("rect").attr("x", function (d, i) {
            return chart_start_padx + d.tw_x;
        }).attr("y", function (d, i) {
            return d.tw_y + boxoffset + padY * i + padY / 2; //pady/2 -> ballance padding
        }).attr("width", function (d) {
            return d.tw_w;
        }).attr("height", function (d) {
            return d.tw_h;
        }).attr("rx", 4)
            .attr("ry", 4)
            .style("fill", "#08C")
            .style("stroke", "rgb(0,0,0)")
            .style("stroke-width", 1)
            .style("stroke-opacity", 0.5);
        ;

        //item duration text
        node.append("text").text(function (d) {
            return d.tw_element.duration + "h";
        }).attr("x", function (d, i) {
            return chart_start_padx + d.tw_x + 5;
        }).attr("y", function (d, i) {
            return d.tw_y + padY * i - 3;
        }).attr("dx", function (d, i) {
            return (d.tw_w / 2) - 5;
        })
            .attr("fill", "white")
            .attr("font-size", "0.8em")
            .attr("text-anchor", "middle");

        drawlines();
    });

    //draw grid lines

    function drawlines() {

        vis.selectAll("linev")
            .data(x.ticks(vertical_ticks))
            .enter().insert("line", ":first-child")
            .attr("x1", x).attr("y1", chart_start_pady)
            .attr("x2", x).attr("y2", chart_height - chart_start_pady - padY-1)
            .attr("width", 1)
            .style("stroke", "#ccc");

        vis.selectAll("header_text")
            .data(x.ticks(vertical_ticks))
            .enter().insert("text", ":first-child")
            .attr("x", x)
            .attr("y", chart_start_pady)
            .attr("dy", -3)
            .attr("text-anchor", "middle")
            .text(function (d, i) { return d; })
            .attr("font-size", "0.8em");

        var y = d3.scale.linear()
            .domain([0, horizontal_ticks * box_height])
            .range([chart_start_pady, chart_height + chart_start_padx]);

        vis.selectAll("lineh")
            .data(y.ticks(horizontal_ticks))
            .enter().insert("line", ":first-child")
            .attr("x1", 0)
            .attr("y1", function (d, i) {
                return chart_start_pady + headeroffset + ((box_height + padY) * i) + i;
            })
            .attr("x2", function (d, i) {
                return chart_width;
            })
            .attr("y2", function (d, i) {
                return chart_start_pady + headeroffset + ((box_height + padY) * i) + i;
            })
            .attr("width", 1)
            .style("stroke", "#ccc");

        vis.selectAll("linex")
            .data([1])
            .enter().insert("line", ":first-child")
            .attr("x1", 0)
            .attr("y1", padY)
            .attr("x2", chart_start_padx + padX)
            .attr("y2", chart_start_pady + headeroffset)
            .attr("width", 1)
            .style("stroke", "#ccc");

        vis.selectAll("header_leg_bottom")
            .data([1])
            .enter().append("text")
            .attr("x", 6)
            .attr("y", 14)
            .attr("dy", -3)
            .attr("text-anchor", "start")
            .text("Activity")
            .attr("font-size", "0.7em")
            .attr("transform", function (d) { return "rotate(28)" /*, " + padX + "," + chart_start_pady + headeroffset + ")"*/; }); ;

        vis.selectAll("header_leg_top")
            .data([1])
            .enter().append("text")
            .attr("x", 20)
            .attr("y", 9)
            .attr("text-anchor", "start")
            .text("Hours")
            .attr("font-size", "0.7em")
        .attr("transform", function (d) { return "rotate(28, 20,9)"; });  
    }

    // Returns a flattened hierarchy containing all leaf nodes under the root.

    function flatten(root) {

        var elements = root.elements;
        var relations = root.relations;
        var flattened = [];

        var lasty = headeroffset;
        $.each(elements, function (index, element) {
            horizontal_ticks += 1;
            lasty = lasty + box_height;

            var e = {
                tw_x: getDurationOffset(element),
                tw_y: lasty + horizontal_ticks + chart_start_pady,
                tw_w: element.duration,
                tw_h: box_height,
                tw_element: element
            };

            flattened.push(e);
            return e;
        });

        var durations = Object();
        $.each(flattened, function (index, elem) {
            if (durations[elem.tw_x] == undefined
                || durations[elem.tw_x] < elem.tw_element.duration) {
                durations[elem.tw_x] = elem.tw_element.duration;
                total_duration += elem.tw_element.duration;
            }
        });

        x = d3.scale.linear()
            .domain([0, total_duration || (chart_width - chart_start_padx) / 10])
            .range([chart_start_padx, chart_width]);
        box_width = x(1) - chart_start_padx;

        $.each(flattened, function (index, elem) {
            elem.tw_w *= box_width;
            elem.tw_x *= box_width;
        });

        function getDurationOffset(elem, acc) {
            var sum = acc || 0;

            var parents = $.grep(relations, function (e, i) { return e.activity == elem.id; });
            if (parents.length > 0) {
                var par = maxParentDuration(parents);
                return getDurationOffset(par, sum + par.duration);
            } else
                return sum;
        }

        function maxParentDuration(parents) {
            var max = 0;
            var parent = 0;
            $.each(parents, function (index, value) {
                if (value.duration > max) {
                    max = value.duration;
                    parent = value;
                }
            });
            return $.grep(elements, function (e, i) { return e.id == parent.parent; })[0];
        }

        return { children: flattened };
    }
}