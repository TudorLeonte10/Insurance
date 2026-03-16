import { useState } from "react"


function DashboardPage() {
    return (
    <div className="space-y-6">

      <h1 className="text-2xl font-bold">Dashboard</h1>

      <div className="grid grid-cols-2 gap-4">

        <div className="bg-white p-6 rounded shadow">
          <h2 className="text-gray-500">Clients</h2>
          <p className="text-3xl font-bold">12</p>
        </div>

        <div className="bg-white p-6 rounded shadow">
          <h2 className="text-gray-500">Policies</h2>
          <p className="text-3xl font-bold">34</p>
        </div>

      </div>

    </div>
  );
}

export default DashboardPage