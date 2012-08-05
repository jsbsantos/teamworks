var tw = tw || { };
tw.Gantt = function (jsonendpoint) {
    //vertical_ticks - multiples of 5
    var verticalTicks = 20, horizontalTicks = 0/*also used as element count*/,
        chartWidth = $("#chart").width() * 0.98/*width=100%*/, chartHeight = 150,
        chartStartPadx = 55, chartStartPady = 20, headeroffset = 10,
        padX = 0, padY = 2,
        boxWidth = 0.1, boxHeight = 13, boxoffset = -boxHeight - 0.5,
        totalDuration = 0;

    var x = undefined; /*d3.scale.linear()
        .domain([0, total_duration || (chart_width - chart_start_padx) / 10])
        .range([chart_start_padx, chart_width]);*/

    var bubble = d3.layout.pack()
        .size(["100%", chartHeight]);

    var vis = d3.select("#chart").append("svg")
        .attr("width", "100%")
        .attr("height", chartHeight + chartStartPady);

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

        //item estimated duration box
        node.append("rect").attr("x", function (d, i) {
            return chartStartPadx + d.tw_x;
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

        node.append("rect").attr("x", function (d, i) {
            return chartStartPadx + d.tw_x + 1;
        }).attr("y", function (d, i) {
            return (d.tw_y + boxoffset + padY * i + padY / 2) + (d.tw_h - 4); //pady/2 -> ballance padding
        }).attr("width", function (d) {
            return d.tw_real_w;
        }).attr("height", 4)
            .attr("rx", 4)
            .attr("ry", 4)
            .style("fill", function (d) {
                if (d.tw_real_w >= d.tw_w * 0.95)
                    return "red";
                return "aqua";
            });

        //item duration text
        node.append("text").text(function (d) {
            return d.tw_element.duration + "h";
        }).attr("x", function (d, i) {
            return chartStartPadx + d.tw_x + 5;
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
            .data(x.ticks(verticalTicks))
            .enter().insert("line", ":first-child")
            .attr("x1", x).attr("y1", chartStartPady)
            .attr("x2", x).attr("y2", chartHeight - chartStartPady - padY - 1)
            .attr("width", 1)
            .style("stroke", "#ccc");

        vis.selectAll("header_text")
            .data(x.ticks(verticalTicks))
            .enter().insert("text", ":first-child")
            .attr("x", x)
            .attr("y", chartStartPady)
            .attr("dy", -3)
            .attr("text-anchor", "middle")
            .text(function (d, i) { return d; })
            .attr("font-size", "0.8em");

        var y = d3.scale.linear()
            .domain([0, horizontalTicks * boxHeight])
            .range([chartStartPady, chartHeight + chartStartPadx]);

        vis.selectAll("lineh")
            .data(y.ticks(horizontalTicks))
            .enter().insert("line", ":first-child")
            .attr("x1", 0)
            .attr("y1", function (d, i) {
                return chartStartPady + headeroffset + ((boxHeight + padY) * i) + i;
            })
            .attr("x2", function (d, i) {
                return chartWidth;
            })
            .attr("y2", function (d, i) {
                return chartStartPady + headeroffset + ((boxHeight + padY) * i) + i;
            })
            .attr("width", 1)
            .style("stroke", "#ccc");

        vis.selectAll("linex")
            .data([1])
            .enter().insert("line", ":first-child")
            .attr("x1", 0)
            .attr("y1", padY)
            .attr("x2", chartStartPadx + padX)
            .attr("y2", chartStartPady + headeroffset)
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
            .attr("transform", function (d) { return "rotate(28)" /*, " + padX + "," + chart_start_pady + headeroffset + ")"*/; });
        ;

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
        var graph = [];

        $.each(relations, function (index, element) {
            graph.push({
                parent: $.grep(elements, function (e, i) { return element.parent == stripEntityName(e.activity); })[0],
                activity: $.grep(elements, function (e, i) { return element.activity == stripEntityName(e.activity); })[0]
            });
        });

        var lasty = headeroffset;
        $.each(elements, function (index, element) {
            horizontalTicks += 1;
            lasty = lasty + boxHeight;

            var e = {
                tw_x: getDurationOffset(element),
                tw_y: lasty + horizontalTicks + chartStartPady,
                tw_w: element.duration,
                tw_h: boxHeight,
                tw_element: element,
                tw_real_w: element.timeused
            };

            flattened.push(e);
            return e;
        });

        var durations = Object();
        $.each(flattened, function (index, elem) {
            if (durations[elem.tw_x] == undefined
                || durations[elem.tw_x] < elem.tw_element.duration) {
                durations[elem.tw_x] = elem.tw_element.duration;
                totalDuration += elem.tw_element.duration;
            }
        });

        x = d3.scale.linear()
            .domain([0, totalDuration || (chartWidth - chartStartPadx) / 10])
            .range([chartStartPadx, chartWidth]);
        boxWidth = x(1) - chartStartPadx;

        $.each(flattened, function (index, elem) {
            elem.tw_w *= boxWidth;
            elem.tw_real_w *= boxWidth;
            elem.tw_x *= boxWidth;
        });

        function getDurationOffset(elem, acc) {
            var sum = acc || 0;

            var parents = $.grep(relations, function (e, i) { return e.activity == stripEntityName(elem.activity); });
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
            return $.grep(elements, function (e, i) { return stripEntityName(e.activity) == parent.parent; })[0];
        }

        function stripEntityName(entity) {
            var splt = entity.toString().lastIndexOf("/") + 1;
            return splt > 1 && entity.substr(splt);
        }

        return { children: flattened };
    }
}