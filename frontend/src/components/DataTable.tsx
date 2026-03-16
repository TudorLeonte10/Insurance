import React from "react";

export interface Column<T> {
  header: string;
  accessor?: keyof T;
  render?: (item: T) => React.ReactNode;
}

interface Props<T extends { id: string | number }> {
  data: T[];
  columns: Column<T>[];
}

function DataTable<T extends { id: string | number }>({ data, columns }: Props<T>) {
  return (
    <div className="bg-white rounded-xl shadow-md border border-gray-200 overflow-hidden">

      <table className="w-full text-sm">

        <thead className="bg-gray-50 text-gray-600">
          <tr>
            {columns.map((col, index) => (
              <th
                key={index}
                className="text-left px-4 py-3 font-semibold"
              >
                {col.header}
              </th>
            ))}
          </tr>
        </thead>

        <tbody>
          {data.map((item) => (
            <tr
              key={item.id}
              className="border-t hover:bg-gray-50 transition"
            >
              {columns.map((col, index) => (
                <td key={index} className="px-4 py-3">

                  {col.render
                    ? col.render(item)
                    : col.accessor
                    ? (item[col.accessor] as React.ReactNode)
                    : null}

                </td>
              ))}
            </tr>
          ))}
        </tbody>

      </table>

    </div>
  );
}

export default DataTable;