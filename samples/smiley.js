let size = 64
let half = size / 2;
let quarter = size / 4;
let eighth = size / 8;
let sixteenth = size / 16;

picture.Width = size;
picture.Height = size;

picture.clear(color.White);

picture.drawCircle(half, half, half - 1, color.Goldenrod);
picture.floodFill(half, half, color.White, color.Yellow);

picture.drawCircle(quarter * 1.5, quarter * 1.25, eighth, color.Black);
picture.floodFill(quarter * 1.5, quarter * 1.25, color.Yellow, color.White);
picture.drawCircle(quarter * 1.5, quarter * 1.5, sixteenth, color.Black);
picture.floodFill(quarter * 1.5, quarter * 1.5, color.White, color.Black);

picture.drawCircle(quarter * 2.5, quarter * 1.25, eighth, color.Black);
picture.floodFill(quarter * 2.5, quarter * 1.25, color.Yellow, color.White);
picture.drawCircle(quarter * 2.5, quarter * 1.5, sixteenth, color.Black);
picture.floodFill(quarter * 2.5, quarter * 1.5, color.White, color.Black);

// picture.drawCircle(half, quarter * 2.5, eighth, color.Black);
picture.drawEllipse(quarter, quarter * 2.25, half, quarter, color.Black);
picture.floodFill(half, quarter * 3, color.Yellow, color.Black);

picture.drawEllipse(quarter, quarter * 1.75, half, quarter, color.Yellow);
picture.floodFill(half, half * 1.25, color.Black, color.Yellow);

//picture.drawCurve(0, 6, 2, 0, 6, 0, color.Black);