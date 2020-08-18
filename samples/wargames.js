if(confirm("SHALL WE PLAY A GAME?"))
{
  let width = 64;
  let height = 64;
  let wWidth = 12;
  let oWidth = 10;
  let pWidth = 8;
  let rWidth = 8;
  let margin = 3;
  let titleWidth = wWidth + oWidth + pWidth + rWidth + (margin * 3);
  let textHeight = 10;
  let textX = Math.round((width - titleWidth) / 2);
  let textY = height - (textHeight + 10);
  let boxX = textX;
  let boxY = 10;
  let boxW = titleWidth - 1;
  let boxH = textY - (boxY + 10);

  picture.width = width;
  picture.height = height;

  picture.lineColor = color.FromArgb(251, 251, 251);
  picture.fillColor = color.FromArgb(124, 146, 124);

  picture.clear();

  picture.fillRectangle(boxX, boxY, boxW, boxH, color.FromArgb(51, 51, 51));
  picture.drawLine(boxX, boxY, boxX + boxW, boxY, color.FromArgb(18, 18, 13));
  picture.drawLine(boxX, boxY, boxX, boxY + boxH, color.FromArgb(41, 41, 32));
  picture.drawLine(boxX, boxY + boxH, boxX + boxW, boxY + boxH, color.FromArgb(162, 159, 112));
  picture.drawLine(boxX + boxW, boxY, boxX + boxW, boxY + boxH, color.FromArgb(18, 19, 16));

  drawW(textX, textY);
  textX += wWidth + 3;
  drawO(textX, textY);
  textX += oWidth + 3;
  drawP(textX, textY);
  textX += pWidth + 3;
  drawR(textX, textY);

  let title = prompt("LOGON:");

  application.Title = title;
}
else
{
  alert("STRANGE GAME");

  application.Quit();
}

function drawW(x, y)
{
  picture.drawLine(x     , y    , x     , y + 3);
  picture.drawLine(x + 1 , y    , x + 1 , y + 6);
  picture.drawLine(x + 2 , y + 4, x + 2 , y + 8);
  picture.drawLine(x + 3 , y + 6, x + 3 , y + 8);
  picture.drawLine(x + 4 , y + 1, x + 4 , y + 7);
  picture.drawLine(x + 5 , y    , x + 5 , y + 4);
  picture.drawLine(x + 6 , y    , x + 6 , y + 4);
  picture.drawLine(x + 7 , y + 1, x + 7 , y + 7);
  picture.drawLine(x + 8 , y + 6, x + 8 , y + 8);
  picture.drawLine(x + 9 , y + 4, x + 9 , y + 8);
  picture.drawLine(x + 10, y    , x + 10, y + 6);
  picture.drawLine(x + 11, y    , x + 11, y + 3);
}

function drawO(x, y)
{
  picture.drawLine(x, y + 3, x, y + 5);
  picture.drawLine(x + 1, y + 1, x + 1, y + 7);
  picture.drawLine(x + 2, y    , x + 2, y + 8);
  picture.drawLine(x + 3, y    , x + 6, y    );
  picture.plot    (x + 3, y + 1);
  picture.plot    (x + 6, y + 1);
  picture.plot    (x + 7, y + 7);
  picture.plot    (x + 3, y + 7);
  picture.drawLine(x + 3, y + 8, x + 7, y + 8);
  picture.drawLine(x + 7, y    , x + 7, y + 2);
  picture.drawLine(x + 8, y + 1, x + 8, y + 8);
  picture.drawLine(x + 9, y + 2, x + 9, y + 6);
  picture.drawLine(x + 4, y + 9, x + 6, y + 9);
}

function drawP(x, y)
{
  picture.drawLine(x    , y    , x    , y + 8);
  picture.drawLine(x + 1, y    , x + 1, y + 8);
  picture.drawLine(x + 2, y    , x + 5, y    );
  picture.drawLine(x + 2, y + 4, x + 5, y + 4);
  picture.drawLine(x + 2, y + 5, x + 5, y + 5);
  picture.plot    (x + 5, y + 1);
  picture.drawLine(x + 6, y    , x + 6, y + 4);
  picture.drawLine(x + 7, y + 1, x + 7, y + 3);
}

function drawR(x, y)
{
  picture.drawLine(x    , y    , x    , y + 8);
  picture.drawLine(x + 1, y    , x + 1, y + 8);
  picture.drawLine(x + 2, y    , x + 5, y    );
  picture.drawLine(x + 2, y + 4, x + 5, y + 4);
  picture.drawLine(x + 2, y + 5, x + 5, y + 5);
  picture.plot    (x + 5, y + 1);
  picture.drawLine(x + 6, y    , x + 6, y + 8);
  picture.plot    (x + 5, y + 6);
  picture.drawLine(x + 7, y + 1, x + 7, y + 3);
  picture.drawLine(x + 7, y + 6, x + 7, y + 8);
}