var TW = TW || { };
TW.Gantt = function (jsonendpoint) {
    var r = 960,
        rw = 770, rh = 150,
        scale = 5,
        padX = 50, padY = 15,
        w = 10, h = 13, headeroffset = 10,
        boxoffset = padY - h - 0.5;

    var bubble = d3.layout.pack()
        .sort(null)
        .size(["100%", rh]);

    var vis = d3.select("#chart").append("svg")
        .attr("width", "100%")
        .attr("height", rh);

    //Draw grid vertical lines
    var enter = vis.selectAll("gridlines")
        .data(d3.range(0, (rw - padX) / scale))
        .enter();

    var header_x1 = padX - w;
    var header_x2 = padX - w;

    enter.append("svg:line")
        .attr("x1", function (d, i) {
            return (header_x1 += w);
        })
        .attr("y1", padY)
        .attr("x2", function (d, i) {
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
    enter.append("text").text(function (d) {
        return d;
    }).attr("x", function (d, i) {
        return (header_x += ((w * 5))) - (d > 9 ? 3.5 : 0);
    }).attr("y", function (d, i) {
        return padY;
    })
        .attr("font-size", "0.8em");

    //draw bars
    d3.json(jsonendpoint, function (json) {
        var node = vis.selectAll("rect")
            .data(bubble.nodes(flat(json))
                    .filter(function (d) { return !d.children; }))
            .enter();

        //item name
        node.append("text").text(function (d) {
            return d.tw_element.name;
        }).attr("x", function (d, i) {
            return 0;
        }).attr("y", function (d, i) {
            return d.tw_y + padY;
        })
        .attr("fill", "black")
        .attr("font-size", "0.8em");

        //grid horizontal line
        node.append("svg:line")
            .attr("x1", 0)
            .attr("y1", function (d, i) {
                return d.tw_y + padY;
            })
            .attr("x2", rw)
            .attr("y2", function (d, i) {
                return d.tw_y + padY;
            })
            .style("stroke", "rgb(0,0,0)")
            .style("stroke-width", 1)
            .style("stroke-opacity", 0.1);

        //item duration box
        node.append("rect").attr("x", function (d, i) {
            return padX + d.tw_x;
        }).attr("y", function (d) {
            return d.tw_y+boxoffset;
        }).attr("width", function (d) {
            return d.tw_w;
        }).attr("height", function (d) {
            return d.tw_h;
        }).attr("rx", 4)
          .attr("ry", 4);

        //item duration text
        node.append("text").text(function (d) {
            return d.tw_element.duration + "h";
        }).attr("x", function (d, i) {
            return padX + d.tw_x + 5;
        }).attr("y", function (d, i) {
            return d.tw_y + padY - 3;
        })
            .attr("fill", "white")
        .attr("font-size", "0.8em");
        
    });

    // Returns a flattened hierarchy containing all leaf nodes under the root.

    function flat(root) {

        var elements = root.elements;
        var relations = root.relations;
        var flattened = [];
        var elementCount = 0;

        var getDepth = function (element, current) {
            var curr = current || 0;

            var elem = $.grep(relations, function (e, i) { return e.activity == element; });
            if (elem.length > 0)
                return getDepth(elem[0], curr + 1);

            return curr + 1;
        };
        var lasty = headeroffset;
        $.each(elements, function (index, element) {
            elementCount += 1;
            lasty = lasty + h;
            var e = {
                tw_x: w * getDurationOffset(element),
                tw_y: lasty + elementCount,
                tw_w: w * element.duration,
                tw_h: h,
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