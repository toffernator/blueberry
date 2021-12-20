import http from "k6/http";
import { check } from "k6";

export const options = {
  vus: 75,
  duration: "3s",
};

export default function () {
  const res = http.get(
    "https://localhost:7104/material?searchKey=Blazor&tags=&startyear=2000&endyear=2021&sortby=NEWEST&type=All"
  );
  check(res, { "status was 200": (r) => r.status == 200 });
}
