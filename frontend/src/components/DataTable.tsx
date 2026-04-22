import React from "react";

export interface Column<T> {
  header: string;
  accessor?: keyof T;
  render?: (item: T) => React.ReactNode;
}

interface DataTableProps<T extends { id: string | number }> {
  data: T[];
  columns: Column<T>[];
}

function DataTable<T extends { id: string | number }>({ data, columns }: DataTableProps<T>) {
  return (
    <div className="bg-white rounded-xl border border-slate-200/80 overflow-hidden shadow-sm">

      <table className="w-full text-sm">

        <thead>
          <tr className="bg-slate-50/80 border-b border-slate-200/80">
            {columns.map((col, index) => (
              <th
                key={index}
                className="text-left px-5 py-3 text-[11px] font-semibold text-slate-500 uppercase tracking-wider"
              >
                {col.header}
              </th>
            ))}
          </tr>
        </thead>

        <tbody>
          {data.map((item, rowIndex) => (
            <tr
              key={item.id}
              className={`
                transition-colors duration-100 border-b border-slate-100 last:border-0
                ${rowIndex % 2 === 1 ? "bg-slate-50/40" : ""}
                hover:bg-teal-50/50
              `}
            >
              {columns.map((col, index) => (
                <td key={index} className="px-5 py-3.5 text-slate-700">

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

      {data.length === 0 && (
        <div className="text-center py-16 text-sm text-slate-400">
          No records found
        </div>
      )}

    </div>
  );
}

export default DataTable;