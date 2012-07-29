var TW = TW || { };
TW.Gantt = function (jsonendpoint, duration) {
    //vertical_ticks - multiples of 5
    var vertical_ticks = 50, horizontal_ticks = 4,
        chart_width = $("#chart").width()/*width=100%*/, chart_height = 150,
        chart_start_padx = 50, chart_start_pady = 15, headeroffset = 10,
        padX = 0, padY = 2,
        box_width = 10, box_height = 13, boxoffset = -box_height - 0.5;

    var elementCount = 0, total_duration=0;

    var bubble = d3.layout.pack()
        .size(["100%", chart_height]);

    var vis = d3.select("#chart").append("svg")
        .attr("width", "100%")
        .attr("height", chart_height);

    var x = d3.scale.linear()
            .domain([0, duration || (chart_width - chart_start_padx) / 10])
            .range([chart_start_padx, chart_width]);

    vis.selectAll("line")
            .data(x.ticks(vertical_ticks))
            .enter().insert("line", ":first-child")
            .attr("x1", x).attr("y1", chart_start_pady)
            .attr("x2", x).attr("y2", 120)
            .attr("width", 1)
            .style("stroke", "#ccc");

    vis.selectAll(".rule")
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
            .attr("x1", chart_start_padx)
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

    /*
    //Draw grid vertical lines
    var enter = vis.selectAll("gridlines")
    .data(d3.range(0, (rw - padX) / scale))
    .enter();

    var header_x1 = padX - w;
    var header_x2 = padX - w;

    enter.append("svg:line")
    .attr("x1", function(d, i) {
    return (header_x1 += w);
    })
    .attr("y1", padY)
    .attr("x2", function(d, i) {
    return (header_x2 += w);
    })
    .attr("y2", rh)
    .style("stroke", "rgb(0,0,0)")
    .style("stroke-width", 1)
    .style("stroke-opacity", 0.1);

    //draw header text
    enter = vis.selectAll("gridheader")
    .data(d3.range(0, (rw - padX) / scale, scale))
    .enter();

    var header_x = padX - (w * 5) - 2;
    enter.append("text").text(function(d) {
    return d;
    }).attr("x", function(d, i) {
    return (header_x += ((w * 5))) - (d > 9 ? 3.5 : 0);
    }).attr("y", function(d, i) {
    return padY;
    })
    .attr("font-size", "0.8em");
    */
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

        //        //grid horizontal line
        //        node.append("svg:line")
        //                .attr("x1", 0)
        //                .attr("y1", function (d, i) {
        //                    return d.tw_y + padY * i;
        //                })
        //                .attr("x2", chart_width)
        //                .attr("y2", function (d, i) {
        //                    return d.tw_y + padY * i;
        //                })
        //                .style("stroke", "rgb(0,0,0)")
        //                .style("stroke-width", 1)
        //                .style("stroke-opacity", 0.1);

        //item duration box
        node.append("rect").attr("x", function (d, i) {
            return chart_start_padx + d.tw_x;
        }).attr("y", function (d, i) {
            return d.tw_y + boxoffset + padY * i +padY/2; //pady/2 -> ballance padding
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
        })
        .attr("fill", "white")
        .attr("font-size", "0.8em");

        //        var y = d3.scale.linear()
        //            .domain([0, elementCount * box_height])
        //            .range([chart_start_pady, chart_height + chart_start_padx]);

        //        vis.selectAll("line")
        //            .data(y.ticks(elementCount))
        //            .enter().append("line")
        //            .attr("x1", 0).attr("y1", chart_start_pady)
        //            .attr("x2", 0).attr("y2", chart_start_padx + y)
        //            .attr("width", 1)
        //            .style("stroke", "red");
        drawlines();
    });

    
   

    // Returns a flattened hierarchy containing all leaf nodes under the root.

    function flatten(root) {

        var elements = root.elements;
        var relations = root.relations;
        var flattened = [];
        
        var lasty = headeroffset;
        $.each(elements, function (index, element) {
            elementCount += 1;
            lasty = lasty + box_height;
            var e = {
                tw_x: box_width * getDurationOffset(element),
                tw_y: lasty + elementCount + chart_start_pady,
                tw_w: box_width * element.duration,
                tw_h: box_height,
                tw_element: element
            };

            flattened.push(e);
            return e;
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