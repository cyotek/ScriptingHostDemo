var sy1;
var sy2;
var cy1;
var cy2;

let width = 128
let height = 64;
let third = height / 3;
let lineColor = color.FromArgb(102, 51, 153);
let lineColor2 = color.FromArgb(153, 51, 102);

picture.Width = width;
picture.Height = height;
picture.clear(color.White);

sy2 = third
cy2 = third * 2;

for(var i = 0; i < width; i++)
{
  sy1 = third + Math.sin(i) * 10;
  cy1 = (third * 2) + Math.cos(i) * 10;

  picture.drawLine(i, sy2, i + 1, sy1, lineColor);
  picture.drawLine(i, cy2, i + 1, cy1, lineColor2);

  sy2 = sy1;
  cy2 = cy1;
}